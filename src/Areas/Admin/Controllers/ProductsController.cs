using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Areas.Admin.Services;
using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class ProductsController : Controller
{
    private readonly IProductAdminService _service;

    public ProductsController(IProductAdminService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string? keyword, int? categoryId, bool? isActive)
    {
        var model = await _service.GetIndexAsync(keyword, categoryId, isActive);
        model.SearchKeyword = keyword;
        model.FilterCategoryId = categoryId;
        model.FilterIsActive = isActive;
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await _service.GetCreateFormAsync();
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductAdminFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm = await _service.GetCreateFormAsync();
            return View(vm);
        }

        try
        {
            await _service.CreateAsync(vm);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            vm = await _service.GetCreateFormAsync();
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetFormAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductAdminFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await _service.GetCategoriesAsync();
            return View(vm);
        }

        try
        {
            var ok = await _service.UpdateAsync(vm);
            if (!ok) return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            vm.Categories = await _service.GetCategoriesAsync();
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        await _service.ToggleActiveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet]
    public async Task<IActionResult> GetProductJson(int id)
    {
        var model = await _service.GetFormAsync(id);
        if (model == null) return NotFound();
        return Json(new
        {
            model.Id,
            model.Name,
            model.Price,
            model.Discount,
            model.StockQuantity,
            model.CategoryId,
            model.Description,
            model.ImageUrl,
            model.GalleryImages,
            model.IsActive
        });
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAjax([FromForm] ProductAdminFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new
            {
                success = false,
                message = string.Join("; ", errors)
            });
        }

        try
        {
            var ok = await _service.UpdateAsync(vm);

            if (!ok)
            {
                return Json(new
                {
                    success = false,
                    message = "Product not found."
                });
            }
        }
        catch (InvalidOperationException ex)
        {
            return Json(new
            {
                success = false,
                message = ex.Message
            });
        }

        return Json(new
        {
            success = true,
            message = "Product updated successfully."
        });
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax([FromForm] ProductAdminFormViewModel vm, bool IsActiveBool = false)
    {
        vm.IsActive = IsActiveBool;

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new
            {
                success = false,
                message = string.Join("; ", errors)
            });
        }

        try
        {
            await _service.CreateAsync(vm);
        }
        catch (InvalidOperationException ex)
        {
            return Json(new
            {
                success = false,
                message = ex.Message
            });
        }

        return Json(new
        {
            success = true,
            message = "Product created successfully."
        });
    }
}