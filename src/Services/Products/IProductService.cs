using MyProject.DTOs.Products;

namespace MyProject.Services.Products;

public interface IProductService
{
    Task<List<ProductListItemDto>> GetAllAsync(string? keyword = null, int? categoryId = null, bool? isActive = null);
    Task<ProductListItemDto?> GetByIdAsync(int id);
    Task<ProductListItemDto> CreateAsync(ProductCreateDto dto);
    Task<bool> UpdateAsync(ProductUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<CategorySelectDto>> GetCategoriesAsync();
}