using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Admin.ViewModels;
using MyProject.Data;

namespace MyProject.Areas.Admin.Services;

public class DashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var totalProducts = await _db.Products.CountAsync();
        var activeProducts = await _db.Products.CountAsync(p => p.IsActive);
        var outOfStock = await _db.Products.CountAsync(p => p.StockQuantity == 0);
        var lowStock = await _db.Products.CountAsync(p => p.StockQuantity > 0 && p.StockQuantity <= 5);

        var totalCategories = await _db.Categories.CountAsync();
        var totalUsers = await _db.Users.CountAsync();
        var activeUsers = await _db.Users.CountAsync(u => u.IsActive);

        var inventoryValue = await _db.Products.SumAsync(p => p.Price * p.StockQuantity);
        var avgPrice = totalProducts > 0 ? await _db.Products.AverageAsync(p => p.Price) : 0;

        var topProducts = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.Price)
            .Take(5)
            .Select(p => new ProductStatItem
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .ToListAsync();

        var lowStockProducts = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.StockQuantity > 0 && p.StockQuantity <= 5)
            .OrderBy(p => p.StockQuantity)
            .Take(5)
            .Select(p => new ProductStatItem
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .ToListAsync();

        var outOfStockProducts = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.StockQuantity == 0)
            .OrderBy(p => p.Name)
            .Take(5)
            .Select(p => new ProductStatItem
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .ToListAsync();

        var categoryStats = await _db.Categories
            .Include(c => c.Products)
            .OrderByDescending(c => c.Products.Count)
            .Select(c => new CategoryStatItem
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.Products.Count
            })
            .ToListAsync();

        var recentUsers = await _db.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(5)
            .Select(u => new RecentUserItem
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        var recentProducts = await _db.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
            .Select(p => new RecentProductItem
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category != null ? p.Category.Name : null,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        var allLowStock = lowStockProducts.Concat(outOfStockProducts).Take(5).ToList();

        return new DashboardViewModel
        {
            TotalProducts = totalProducts,
            ActiveProducts = activeProducts,
            OutOfStockProducts = outOfStock,
            LowStockCount = lowStock,
            TotalCategories = totalCategories,
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            TotalInventoryValue = inventoryValue,
            AverageProductPrice = avgPrice,
            TopProducts = topProducts,
            LowStockProducts = allLowStock,
            CategoryStats = categoryStats,
            RecentUsers = recentUsers,
            RecentProducts = recentProducts
        };
    }
}
