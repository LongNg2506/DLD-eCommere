using Microsoft.AspNetCore.Mvc;
using MyProject.DTOs.Shop;
using MyProject.Services.Shop;

namespace MyProject.Controllers;

public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? q,
        List<int>? categories,
        decimal? minPrice,
        decimal? maxPrice,
        string sort = "newest",
        int page = 1)
    {
        var filter = new ShopFilterDto
        {
            Keyword = q,
            CategoryIds = categories,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SortBy = sort,
            Page = page,
            PageSize = 12
        };

        var result = await _shopService.GetProductsAsync(filter);
        var categoriesList = await _shopService.GetCategoriesAsync();

        var viewModel = new ShopIndexViewModel
        {
            Result = result,
            Categories = categoriesList
        };

        return View(viewModel);
    }

    [HttpGet("Shop/Product/{id:int}")]
    public async Task<IActionResult> ProductDetail(int id)
    {
        var product = await _shopService.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        var relatedProducts = await _shopService.GetProductsAsync(new ShopFilterDto
        {
            CategoryIds = new List<int> { product.CategoryId },
            Page = 1,
            PageSize = 4
        });

        var viewModel = new ShopProductDetailViewModel
        {
            Product = product,
            RelatedProducts = relatedProducts.Products.Where(p => p.Id != id).ToList()
        };

        return View("ProductDetail", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Filter([FromQuery] ShopFilterDto filter)
    {
        filter.PageSize = 12;
        var result = await _shopService.GetProductsAsync(filter);
        return PartialView("_ProductGrid", result);
    }
}

public class ShopIndexViewModel
{
    public ShopResultDto Result { get; set; } = new();
    public List<ShopCategoryDto> Categories { get; set; } = new();
}

public class ShopProductDetailViewModel
{
    public ShopProductDto Product { get; set; } = new();
    public List<ShopProductDto> RelatedProducts { get; set; } = new();
}