using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Admin.ViewModels;
using MyProject.Data;

namespace MyProject.Areas.Admin.Services;

public class UserAdminService : IUserAdminService
{
    private readonly AppDbContext _db;

    public UserAdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserAdminIndexViewModel> GetIndexAsync(string? role, bool? isActive, string? keyword)
    {
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(u => u.Role == role);

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(u =>
                u.FullName.Contains(keyword) ||
                u.Email.Contains(keyword) ||
                (u.Phone != null && u.Phone.Contains(keyword)));

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserAdminItem
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        var total = await _db.Users.CountAsync();
        var active = await _db.Users.CountAsync(u => u.IsActive);

        return new UserAdminIndexViewModel
        {
            Users = users,
            TotalCount = total,
            ActiveCount = active,
            InactiveCount = total - active
        };
    }

    public async Task<UserAdminDetailViewModel?> GetByIdAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return null;

        return new UserAdminDetailViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateRoleAsync(int id, string role)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        user.Role = role;
        user.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return true;
    }
}