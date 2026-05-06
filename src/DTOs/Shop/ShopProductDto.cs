namespace MyProject.DTOs.Shop;

public class ShopProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public int StockQuantity { get; set; }
    public string? CategoryName { get; set; }
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? GalleryImages { get; set; }
    public List<string> GalleryList => string.IsNullOrWhiteSpace(GalleryImages)
        ? new List<string>()
        : GalleryImages.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public decimal SalePrice => Price - (Price * Discount / 100);

    public string Badge
    {
        get
        {
            if (Discount >= 20) return "hot";
            if (CreatedAt > DateTime.Now.AddDays(-14)) return "new";
            if (Discount > 0) return "sale";
            return "none";
        }
    }
}
