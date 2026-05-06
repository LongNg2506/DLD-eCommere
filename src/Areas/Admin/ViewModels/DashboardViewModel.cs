namespace MyProject.Areas.Admin.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public int LowStockCount { get; set; }
    public int TotalCategories { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public decimal AverageProductPrice { get; set; }
    public List<ProductStatItem> TopProducts { get; set; } = new();
    public List<ProductStatItem> LowStockProducts { get; set; } = new();
    public List<CategoryStatItem> CategoryStats { get; set; } = new();
    public List<RecentUserItem> RecentUsers { get; set; } = new();
    public List<RecentProductItem> RecentProducts { get; set; } = new();
}

public class ProductStatItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal SalePrice => Discount > 0 ? Price - (Price * Discount / 100) : Price;
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public string? CategoryName { get; set; }
}

public class CategoryStatItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}

public class RecentUserItem
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RecentProductItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
