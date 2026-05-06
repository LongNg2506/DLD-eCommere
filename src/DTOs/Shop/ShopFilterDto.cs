namespace MyProject.DTOs.Shop;

public class ShopFilterDto
{
    public string? Keyword { get; set; }
    public List<int>? CategoryIds { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string SortBy { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
