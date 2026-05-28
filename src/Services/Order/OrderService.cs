using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

using OrderModel = MyProject.Models.Order;
using PaymentModel = MyProject.Models.Payment;

namespace MyProject.Services.Order;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var cartId = await GetCartIdAsync(request.SessionId, request.UserId);
            if (cartId == null)
                return new OrderResult { Success = false, Message = "Cart not found." };

            var cartItems = await _db.CartSessionItems
                .Include(i => i.Product)
                .Where(i => i.CartSessionId == cartId)
                .ToListAsync();

            if (!cartItems.Any())
                return new OrderResult { Success = false, Message = "Cart is empty." };

            // Kiểm tra và trừ tồn kho ngay lập tức để tránh Race Condition
            foreach (var item in cartItems)
            {
                if (item.Product == null || item.Product.StockQuantity < item.Quantity)
                {
                    var name = item.Product?.Name ?? "Unknown";
                    var available = item.Product?.StockQuantity ?? 0;
                    return new OrderResult
                    {
                        Success = false,
                        Message = $"Product \"{name}\" only has {available} items in stock. Please adjust your order."
                    };
                }
                // Trừ kho ngay tại đây
                item.Product!.StockQuantity -= item.Quantity;
            }

            var invoiceNumber = await GenerateInvoiceNumberAsync();
            var order = new OrderModel
            {
                UserId = request.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = cartItems.Sum(i => (i.Product!.Price * (1 - i.Product.Discount / 100)) * i.Quantity),
                ShippingAddress = request.ShippingAddress,
                Phone = request.Phone,
                Note = request.Note,
                CustomerName = request.CustomerName,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = "Pending",
                InvoiceNumber = invoiceNumber,
                CreatedAt = DateTime.Now
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                if (item.Product == null) continue;
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price * (1 - item.Product.Discount / 100),
                    ProductName = item.Product.Name
                });
            }

            _db.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = "Pending",
                Note = "Order placed",
                UpdatedBy = request.CustomerName,
                CreatedAt = DateTime.Now
            });

            if (request.PaymentMethod != "COD")
            {
                _db.Payments.Add(new PaymentModel
                {
                    OrderId = order.Id,
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = "Pending",
                    Amount = order.TotalAmount,
                    CreatedAt = DateTime.Now
                });
            }

            _db.CartSessionItems.RemoveRange(cartItems);

            var cart = await _db.CartSessions.FindAsync(cartId);
            if (cart != null)
                _db.CartSessions.Remove(cart);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return new OrderResult
            {
                Success = true,
                Message = "Order placed successfully!",
                OrderId = order.Id,
                InvoiceNumber = invoiceNumber
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new OrderResult { Success = false, Message = $"An error occurred: {ex.Message}" };
        }
    }

    public async Task<OrderDetailDto?> GetOrderAsync(int orderId, int? userId)
    {
        var order = await _db.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            return null;

        if (userId != null && order.UserId != userId)
            return null;

        if (userId == null && order.UserId != null)
            return null;

        return new OrderDetailDto
        {
            Id = order.Id,
            InvoiceNumber = order.InvoiceNumber ?? $"ORD{order.Id:D5}",
            Status = order.Status,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            CustomerName = order.CustomerName ?? "",
            Phone = order.Phone,
            ShippingAddress = order.ShippingAddress,
            Note = order.Note,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ImageUrl = i.Product?.ImageUrl,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),
            StatusHistory = order.StatusHistory
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => new OrderStatusHistoryDto
                {
                    Status = h.Status,
                    Note = h.Note,
                    UpdatedBy = h.UpdatedBy,
                    CreatedAt = h.CreatedAt
                }).ToList()
        };
    }

    public async Task<List<OrderListDto>> GetUserOrdersAsync(int userId)
    {
        return await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderListDto
            {
                Id = o.Id,
                InvoiceNumber = o.InvoiceNumber ?? $"ORD{o.Id:D5}",
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ItemCount = o.OrderItems.Sum(i => i.Quantity)
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(int orderId, string status, string? updatedBy, string? note)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null)
            return false;

        order.Status = status;
        _db.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = status,
            Note = note,
            UpdatedBy = updatedBy,
            CreatedAt = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeductStockAsync(int orderId)
    {
        var items = await _db.OrderItems
            .Include(i => i.Product)
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        foreach (var item in items)
        {
            if (item.Product != null)
            {
                item.Product.StockQuantity -= item.Quantity;
            }
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<string> GenerateInvoiceNumberAsync()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        return $"INV-{today}-{uniqueId}";
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
