using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Admin.ViewModels;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Areas.Admin.Services;

public class CategoryAdminService : ICategoryAdminService
{
    private readonly AppDbContext _db;

    public CategoryAdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CategoryAdminIndexViewModel> GetAllAsync()
    {
        var categories = await _db.Categories
            .Include(c => c.Products)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryAdminItem
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return new CategoryAdminIndexViewModel
        {
            Categories = categories,
            TotalCount = categories.Count
        };
    }

    public async Task<CategoryAdminFormViewModel?> GetByIdAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return null;

        return new CategoryAdminFormViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryAdminItem> CreateAsync(CategoryAdminFormViewModel vm)
    {
        var category = new Category
        {
            Name = vm.Name.Trim(),
            Description = vm.Description,
            CreatedAt = DateTime.Now
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return new CategoryAdminItem
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ProductCount = 0,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(CategoryAdminFormViewModel vm)
    {
        var category = await _db.Categories.FindAsync(vm.Id);
        if (category == null) return false;

        category.Name = vm.Name.Trim();
        category.Description = vm.Description;
        category.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _db.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null) return false;

        if (category.Products.Any())
            throw new InvalidOperationException("Cannot delete a category that has products.");

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return true;
    }
}