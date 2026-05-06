using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.DTOs.Shop;

namespace MyProject.Services.Shop;

public class ShopService : IShopService
{
    private readonly AppDbContext _db;

    public ShopService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ShopResultDto> GetProductsAsync(ShopFilterDto filter)
    {
        var query = _db.Products
            .Include(x => x.Category)
            .Where(x => x.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            query = query.Where(x =>
                x.Name.Contains(filter.Keyword) ||
                (x.Description != null && x.Description.Contains(filter.Keyword)));
        }

        if (filter.CategoryIds != null && filter.CategoryIds.Count > 0)
        {
            query = query.Where(x => filter.CategoryIds.Contains(x.CategoryId));
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(x => (x.Price - (x.Price * x.Discount / 100)) >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue && filter.MaxPrice.Value > 0)
        {
            query = query.Where(x => (x.Price - (x.Price * x.Discount / 100)) <= filter.MaxPrice.Value);
        }

        query = filter.SortBy switch
        {
            "price_asc" => query.OrderBy(x => x.Price - (x.Price * x.Discount / 100)),
            "price_desc" => query.OrderByDescending(x => x.Price - (x.Price * x.Discount / 100)),
            "name_asc" => query.OrderBy(x => x.Name),
            "oldest" => query.OrderBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize);

        var products = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new ShopProductDto
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
                GalleryImages = x.GalleryImages,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return new ShopResultDto
        {
            Products = products,
            TotalItems = totalItems,
            TotalPages = totalPages,
            CurrentPage = filter.Page,
            PageSize = filter.PageSize,
            Filter = filter
        };
    }

    public async Task<ShopProductDto?> GetProductByIdAsync(int id)
    {
        return await _db.Products
            .Include(x => x.Category)
            .Where(x => x.Id == id && x.IsActive)
            .Select(x => new ShopProductDto
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
                GalleryImages = x.GalleryImages,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ShopCategoryDto>> GetCategoriesAsync()
    {
        return await _db.Categories
            .Select(x => new ShopCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                ProductCount = x.Products.Count(p => p.IsActive)
            })
            .ToListAsync();
    }
}
