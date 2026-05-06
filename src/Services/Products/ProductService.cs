using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.DTOs.Products;
using MyProject.Models;

namespace MyProject.Services.Products;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductListItemDto>> GetAllAsync(string? keyword = null, int? categoryId = null, bool? isActive = null)
    {
        var query = _db.Products
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Discount = x.Discount,
                StockQuantity = x.StockQuantity,
                CategoryName = x.Category != null ? x.Category.Name : null,
                CategoryId = x.CategoryId,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ProductListItemDto?> GetByIdAsync(int id)
    {
        return await _db.Products
            .Include(x => x.Category)
            .Where(x => x.Id == id)
            .Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Discount = x.Discount,
                StockQuantity = x.StockQuantity,
                CategoryName = x.Category != null ? x.Category.Name : null,
                CategoryId = x.CategoryId,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductListItemDto> CreateAsync(ProductCreateDto dto)
    {
        var categoryExists = await _db.Categories.AnyAsync(x => x.Id == dto.CategoryId);
        if (!categoryExists)
        {
            throw new InvalidOperationException("Danh mục không tồn tại.");
        }

        var product = new Product
        {
            Name = dto.Name.Trim(),
            Price = dto.Price,
            Discount = dto.Discount,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.Now
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return new ProductListItemDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(ProductUpdateDto dto)
    {
        var product = await _db.Products.FindAsync(dto.Id);
        if (product == null) return false;

        var categoryExists = await _db.Categories.AnyAsync(x => x.Id == dto.CategoryId);
        if (!categoryExists)
        {
            throw new InvalidOperationException("Danh mục không tồn tại.");
        }

        product.Name = dto.Name.Trim();
        product.Price = dto.Price;
        product.Discount = dto.Discount;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
        product.Description = dto.Description;
        product.ImageUrl = dto.ImageUrl;
        product.IsActive = dto.IsActive;
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

    public async Task<List<CategorySelectDto>> GetCategoriesAsync()
    {
        return await _db.Categories
            .OrderBy(x => x.Name)
            .Select(x => new CategorySelectDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}