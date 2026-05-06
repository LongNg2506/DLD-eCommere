using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Services;

public interface ICategoryAdminService
{
    Task<CategoryAdminIndexViewModel> GetAllAsync();
    Task<CategoryAdminFormViewModel?> GetByIdAsync(int id);
    Task<CategoryAdminItem> CreateAsync(CategoryAdminFormViewModel vm);
    Task<bool> UpdateAsync(CategoryAdminFormViewModel vm);
    Task<bool> DeleteAsync(int id);
}