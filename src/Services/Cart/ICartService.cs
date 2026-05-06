namespace MyProject.Services.Cart;

public interface ICartService
{
    Task<CartResult> GetCartAsync(string sessionId, int? userId);
    Task<CartItemResult> AddItemAsync(string sessionId, int? userId, int productId, int quantity);
    Task<CartItemResult> UpdateQuantityAsync(string sessionId, int? userId, int productId, int quantity);
    Task<bool> RemoveItemAsync(string sessionId, int? userId, int productId);
    Task<bool> ClearCartAsync(string sessionId, int? userId);
    Task<int> GetCartCountAsync(string sessionId, int? userId);
    Task MergeCartAsync(string guestSessionId, int userId);
    Task<bool> ValidateStockAsync(string sessionId, int? userId);
}

public class CartItemResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public CartItemDto? Item { get; set; }
}

public class CartResult
{
    public List<CartItemDto> Items { get; set; } = new();
    public decimal Total { get; set; }
    public int Count { get; set; }
}

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
    public int StockQuantity { get; set; }
    public decimal Subtotal => SalePrice * Quantity;
}
