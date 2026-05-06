using MyProject.DTOs.Products;

namespace MyProject.Models;

public class HomeViewModel
{
    public List<ProductListItemDto> FeaturedProducts { get; set; } = new();
    public List<ProductListItemDto> NewArrivals { get; set; } = new();
    public List<ProductListItemDto> BestSellers { get; set; } = new();
    public List<CategoryDisplayDto> Categories { get; set; } = new();
    public List<ProductListItemDto> FlashDeals { get; set; } = new();
    public int CartItemCount { get; set; }
}

public class CategoryDisplayDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
