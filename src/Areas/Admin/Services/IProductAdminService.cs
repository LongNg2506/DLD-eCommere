using MyProject.Areas.Admin.ViewModels;
using MyProject.DTOs.Products;

namespace MyProject.Areas.Admin.Services;

public interface IProductAdminService
{
    Task<ProductAdminIndexViewModel> GetIndexAsync(string? keyword, int? categoryId, bool? isActive, int page = 1, int pageSize = 10);
    Task<ProductAdminFormViewModel?> GetFormAsync(int id);
    Task<ProductAdminFormViewModel> GetCreateFormAsync();
    Task<ProductAdminItem> CreateAsync(ProductAdminFormViewModel vm);
    Task<bool> UpdateAsync(ProductAdminFormViewModel vm);
    Task<bool> DeleteAsync(int id);
    Task<bool> ToggleActiveAsync(int id);
    Task<List<CategorySelectDto>> GetCategoriesAsync();
}