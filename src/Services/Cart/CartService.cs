using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Services.Cart;

public class CartService : ICartService
{
    private readonly AppDbContext _db;

    public CartService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CartResult> GetCartAsync(string sessionId, int? userId)
    {
        var cartId = await GetOrCreateCartIdAsync(sessionId, userId);
        if (cartId == null)
            return new CartResult();

        var items = await _db.CartSessionItems
            .Where(i => i.CartSessionId == cartId)
            .Include(i => i.Product)
            .Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product != null ? i.Product.Name : "",
                ImageUrl = i.Product != null ? i.Product.ImageUrl : null,
                UnitPrice = i.Product != null ? i.Product.Price : 0,
                SalePrice = i.Product != null ? i.Product.Price * (1 - i.Product.Discount / 100) : 0,
                Discount = i.Product != null ? i.Product.Discount : 0,
                Quantity = i.Quantity,
                StockQuantity = i.Product != null ? i.Product.StockQuantity : 0
            })
            .ToListAsync();

        return new CartResult
        {
            Items = items,
            Total = items.Sum(i => i.Subtotal),
            Count = items.Sum(i => i.Quantity)
        };
    }

    public async Task<CartItemResult> AddItemAsync(string sessionId, int? userId, int productId, int quantity)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null || !product.IsActive)
            return new CartItemResult { Success = false, Message = "Product not found." };

        if (product.StockQuantity < quantity)
            return new CartItemResult { Success = false, Message = $"Only {product.StockQuantity} items in stock." };

        var cartId = await GetOrCreateCartIdAsync(sessionId, userId);

        var existingItem = await _db.CartSessionItems
            .FirstOrDefaultAsync(i => i.CartSessionId == cartId && i.ProductId == productId);

        if (existingItem != null)
        {
            var newQty = existingItem.Quantity + quantity;
            if (product.StockQuantity < newQty)
                return new CartItemResult { Success = false, Message = $"Cannot add more. Only {product.StockQuantity} items available." };

            existingItem.Quantity = newQty;
            await _db.SaveChangesAsync();
        }
        else
        {
            var item = new CartSessionItem
            {
                CartSessionId = cartId!.Value,
                ProductId = productId,
                Quantity = quantity,
                AddedAt = DateTime.Now
            };
            _db.CartSessionItems.Add(item);
            await _db.SaveChangesAsync();
            existingItem = item;
        }

        return new CartItemResult
        {
            Success = true,
            Message = $"{product.Name} added to cart!",
            Item = new CartItemDto
            {
                Id = existingItem.Id,
                ProductId = productId,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl,
                UnitPrice = product.Price,
                SalePrice = product.Price * (1 - product.Discount / 100),
                Discount = product.Discount,
                Quantity = existingItem.Quantity,
                StockQuantity = product.StockQuantity
            }
        };
    }

    public async Task<CartItemResult> UpdateQuantityAsync(string sessionId, int? userId, int productId, int quantity)
    {
        if (quantity <= 0)
            return await RemoveItemAsync(sessionId, userId, productId)
                ? new CartItemResult { Success = true, Message = "Item removed from cart." }
                : new CartItemResult { Success = false, Message = "Item not found." };

        var cartId = await GetCartIdAsync(sessionId, userId);
        if (cartId == null)
            return new CartItemResult { Success = false, Message = "Cart not found." };

        var item = await _db.CartSessionItems
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.CartSessionId == cartId && i.ProductId == productId);

        if (item == null)
            return new CartItemResult { Success = false, Message = "Item not found." };

        if (item.Product != null && item.Product.StockQuantity < quantity)
            return new CartItemResult { Success = false, Message = $"Only {item.Product.StockQuantity} items in stock." };

        item.Quantity = quantity;
        await _db.SaveChangesAsync();

        return new CartItemResult
        {
            Success = true,
            Message = "Quantity updated.",
            Item = new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "",
                ImageUrl = item.Product?.ImageUrl,
                UnitPrice = item.Product?.Price ?? 0,
                SalePrice = item.Product != null ? item.Product.Price * (1 - item.Product.Discount / 100) : 0,
                Discount = item.Product?.Discount ?? 0,
                Quantity = item.Quantity,
                StockQuantity = item.Product?.StockQuantity ?? 0
            }
        };
    }

    public async Task<bool> RemoveItemAsync(string sessionId, int? userId, int productId)
    {
        var cartId = await GetCartIdAsync(sessionId, userId);
        if (cartId == null)
            return false;

        var item = await _db.CartSessionItems
            .FirstOrDefaultAsync(i => i.CartSessionId == cartId && i.ProductId == productId);

        if (item == null)
            return false;

        _db.CartSessionItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(string sessionId, int? userId)
    {
        var cartId = await GetCartIdAsync(sessionId, userId);
        if (cartId == null)
            return true;

        var items = await _db.CartSessionItems
            .Where(i => i.CartSessionId == cartId)
            .ToListAsync();

        _db.CartSessionItems.RemoveRange(items);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetCartCountAsync(string sessionId, int? userId)
    {
        var cartId = await GetCartIdAsync(sessionId, userId);
        if (cartId == null)
            return 0;

        return await _db.CartSessionItems
            .Where(i => i.CartSessionId == cartId)
            .SumAsync(i => i.Quantity);
    }

    public async Task MergeCartAsync(string guestSessionId, int userId)
    {
        var guestCart = await _db.CartSessions
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == guestSessionId && c.UserId == null);

        if (guestCart == null)
            return;

        var userCart = await _db.CartSessions
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (userCart == null)
        {
            guestCart.UserId = userId;
            guestCart.SessionId = null;
            guestCart.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return;
        }

        foreach (var guestItem in guestCart.Items)
        {
            var userItem = userCart.Items.FirstOrDefault(i => i.ProductId == guestItem.ProductId);
            if (userItem != null)
            {
                userItem.Quantity += guestItem.Quantity;
            }
            else
            {
                _db.CartSessionItems.Add(new CartSessionItem
                {
                    CartSessionId = userCart.Id,
                    ProductId = guestItem.ProductId,
                    Quantity = guestItem.Quantity,
                    AddedAt = DateTime.Now
                });
            }
        }

        _db.CartSessionItems.RemoveRange(guestCart.Items);
        _db.CartSessions.Remove(guestCart);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ValidateStockAsync(string sessionId, int? userId)
    {
        var cartId = await GetCartIdAsync(sessionId, userId);
        if (cartId == null)
            return true;

        var items = await _db.CartSessionItems
            .Include(i => i.Product)
            .Where(i => i.CartSessionId == cartId)
            .ToListAsync();

        foreach (var item in items)
        {
            if (item.Product == null || item.Product.StockQuantity < item.Quantity)
                return false;
        }

        return true;
    }

    private async Task<int?> GetOrCreateCartIdAsync(string sessionId, int? userId)
    {
        var cart = await _db.CartSessions
            .FirstOrDefaultAsync(c =>
                (userId != null && c.UserId == userId) ||
                (userId == null && c.SessionId == sessionId));

        if (cart != null)
        {
            cart.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return cart.Id;
        }

        var newCart = new CartSession
        {
            SessionId = userId == null ? sessionId : null,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _db.CartSessions.Add(newCart);
        await _db.SaveChangesAsync();
        return newCart.Id;
    }

    private async Task<int?> GetCartIdAsync(string sessionId, int? userId)
    {
        var cart = await _db.CartSessions
            .FirstOrDefaultAsync(c =>
                (userId != null && c.UserId == userId) ||
                (userId == null && c.SessionId == sessionId));
        return cart?.Id;
    }
}
