using Microsoft.AspNetCore.Mvc;
using MyProject.DTOs.Products;
using MyProject.Services.Products;

namespace MyProject.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? keyword, int? categoryId, bool? isActive)
    {
        var products = await _productService.GetAllAsync(keyword, categoryId, isActive);
        var categories = await _productService.GetCategoriesAsync();

        ViewBag.Keyword = keyword;
        ViewBag.CategoryId = categoryId;
        ViewBag.IsActive = isActive;
        ViewBag.Categories = categories;

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _productService.GetCategoriesAsync();
        ViewBag.Categories = categories;
        return View(new ProductCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _productService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View(dto);
        }

        try
        {
            await _productService.CreateAsync(dto);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            var cats = await _productService.GetCategoriesAsync();
            ViewBag.Categories = cats;
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        var categories = await _productService.GetCategoriesAsync();
        ViewBag.Categories = categories;

        var dto = new ProductUpdateDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _productService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View(dto);
        }

        try
        {
            var updated = await _productService.UpdateAsync(dto);
            if (!updated) return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            var cats = await _productService.GetCategoriesAsync();
            ViewBag.Categories = cats;
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
