using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Admin.ViewModels;
using MyProject.Data;
using MyProject.DTOs.Products;
using MyProject.Models;

namespace MyProject.Areas.Admin.Services;

public class ProductAdminService : IProductAdminService
{
    private readonly AppDbContext _db;

    public ProductAdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProductAdminIndexViewModel> GetIndexAsync(string? keyword, int? categoryId, bool? isActive)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(p => p.Name.Contains(keyword));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductAdminItem
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                CategoryName = p.Category != null ? p.Category.Name : null,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        var total = await _db.Products.CountAsync();
        var active = await _db.Products.CountAsync(p => p.IsActive);
        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategorySelectDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return new ProductAdminIndexViewModel
        {
            Products = products,
            TotalCount = total,
            ActiveCount = active,
            InactiveCount = total - active,
            Categories = categories
        };
    }

    public async Task<ProductAdminFormViewModel?> GetFormAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;

        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategorySelectDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return new ProductAdminFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            GalleryImages = product.GalleryImages,
            IsActive = product.IsActive,
            Categories = categories
        };
    }

    public async Task<ProductAdminFormViewModel> GetCreateFormAsync()
    {
        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategorySelectDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return new ProductAdminFormViewModel
        {
            Categories = categories,
            IsActive = true
        };
    }

    public async Task<ProductAdminItem> CreateAsync(ProductAdminFormViewModel vm)
    {
        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == vm.CategoryId);
        if (!categoryExists)
            throw new InvalidOperationException("Danh mục không tồn tại.");

        var product = new Product
        {
            Name = vm.Name.Trim(),
            Price = vm.Price,
            Discount = vm.Discount,
            StockQuantity = vm.StockQuantity,
            CategoryId = vm.CategoryId,
            Description = vm.Description,
            ImageUrl = vm.ImageUrl,
            GalleryImages = vm.GalleryImages,
            IsActive = vm.IsActive,
            CreatedAt = DateTime.Now
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return new ProductAdminItem
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl
        };
    }

    public async Task<bool> UpdateAsync(ProductAdminFormViewModel vm)
    {
        var product = await _db.Products.FindAsync(vm.Id);
        if (product == null) return false;

        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == vm.CategoryId);
        if (!categoryExists)
            throw new InvalidOperationException("Danh mục không tồn tại.");

        product.Name = vm.Name.Trim();
        product.Price = vm.Price;
        product.Discount = vm.Discount;
        product.StockQuantity = vm.StockQuantity;
        product.CategoryId = vm.CategoryId;
        product.Description = vm.Description;
        product.ImageUrl = vm.ImageUrl;
        product.GalleryImages = vm.GalleryImages;
        product.IsActive = vm.IsActive;
        product.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;

        product.IsActive = !product.IsActive;
        product.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<CategorySelectDto>> GetCategoriesAsync()
    {
        return await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategorySelectDto { Id = c.Id, Name = c.Name })
            .ToListAsync();
    }
}