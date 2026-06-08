using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.DTOs.Products;
using MyProject.Models;
using MyProject.Services.Products;

namespace MyProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly IProductService _productService;

    private static readonly string[] WatchImages = new[]
    {
        "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
        "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
        "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80",
        "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
        "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80",
        "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
        "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
        "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80",
        "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80",
        "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80",
        "https://images.unsplash.com/photo-1622434641406-a158123450f9?w=600&q=80",
        "https://images.unsplash.com/photo-1594534475808-b18fc33b045e?w=600&q=80",
    };

    public HomeController(AppDbContext db, IProductService productService)
    {
        _db = db;
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? keyword, int? categoryId, string? sortBy)
    {
        var allProducts = await _productService.GetAllAsync(keyword, categoryId, isActive: true);

        int imgIndex = 0;
        foreach (var p in allProducts)
        {
            if (string.IsNullOrWhiteSpace(p.ImageUrl))
                p.ImageUrl = WatchImages[imgIndex % WatchImages.Length];
            imgIndex++;
        }

        var vm = new HomeViewModel
        {
            FeaturedProducts = allProducts.Take(20).ToList(),
            NewArrivals = allProducts.OrderByDescending(p => p.CreatedAt).Take(20).ToList(),
            BestSellers = allProducts.OrderByDescending(p => p.SalePrice).Take(20).ToList(),
            FlashDeals = allProducts.Where(p => p.Discount > 0).OrderByDescending(p => p.Discount).Take(20).ToList(),
            Categories = BuildCategoryDisplays()
        };

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            vm.FeaturedProducts = allProducts;
            vm.NewArrivals = allProducts;
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q, int? cat, string sort)
    {
        var results = await _productService.GetAllAsync(q, cat, isActive: true);

        int imgIdx = 0;
        foreach (var p in results)
        {
            if (string.IsNullOrWhiteSpace(p.ImageUrl))
                p.ImageUrl = WatchImages[imgIdx % WatchImages.Length];
            imgIdx++;
        }

        results = sort switch
        {
            "price_asc" => results.OrderBy(p => p.SalePrice).ToList(),
            "price_desc" => results.OrderByDescending(p => p.SalePrice).ToList(),
            "name" => results.OrderBy(p => p.Name).ToList(),
            "newest" => results.OrderByDescending(p => p.CreatedAt).ToList(),
            "discount" => results.OrderByDescending(p => p.Discount).ToList(),
            _ => results
        };

        return View("Index", new HomeViewModel
        {
            FeaturedProducts = results,
            Categories = BuildCategoryDisplays()
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private List<CategoryDisplayDto> BuildCategoryDisplays()
    {
        return new List<CategoryDisplayDto>
        {
            new() { Id = 1, Name = "Men's Watches",     Icon = "men",   Color = "#4caf50" },
            new() { Id = 2, Name = "Women's Watches",  Icon = "women", Color = "#e91e63" },
            new() { Id = 3, Name = "Couple Watches",    Icon = "couple",Color = "#9c27b0" },
            new() { Id = 4, Name = "Luxury Watches",    Icon = "luxury",Color = "#ff9800" },
            new() { Id = 5, Name = "Smart Watches",     Icon = "smart", Color = "#2196f3" },
            new() { Id = 6, Name = "Sport Watches",     Icon = "sport", Color = "#f44336" },
            new() { Id = 7, Name = "Casual Watches",    Icon = "casual",Color = "#00bcd4" },
            new() { Id = 8, Name = "Dress Watches",     Icon = "dress", Color = "#795548" }
        };
    }
}
