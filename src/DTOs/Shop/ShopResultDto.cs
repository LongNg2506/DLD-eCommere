namespace MyProject.DTOs.Shop;

public class ShopResultDto
{
    public List<ShopProductDto> Products { get; set; } = new();
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public ShopFilterDto Filter { get; set; } = new();
}
