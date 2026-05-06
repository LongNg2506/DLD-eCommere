using MyProject.DTOs.Shop;

namespace MyProject.Services.Shop;

public interface IShopService
{
    Task<ShopResultDto> GetProductsAsync(ShopFilterDto filter);
    Task<ShopProductDto?> GetProductByIdAsync(int id);
    Task<List<ShopCategoryDto>> GetCategoriesAsync();
}