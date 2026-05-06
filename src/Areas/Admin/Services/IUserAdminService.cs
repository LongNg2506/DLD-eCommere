using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Services;

public interface IUserAdminService
{
    Task<UserAdminIndexViewModel> GetIndexAsync(string? role, bool? isActive, string? keyword);
    Task<UserAdminDetailViewModel?> GetByIdAsync(int id);
    Task<bool> ToggleActiveAsync(int id);
    Task<bool> UpdateRoleAsync(int id, string role);
}