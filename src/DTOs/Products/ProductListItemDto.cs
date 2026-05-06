namespace MyProject.DTOs.Products;

public class ProductListItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal Discount { get; set; }

    public decimal SalePrice => Price - (Price * Discount / 100);

    public int StockQuantity { get; set; }

    public string? CategoryName { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
