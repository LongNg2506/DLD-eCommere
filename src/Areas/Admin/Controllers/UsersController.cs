using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Areas.Admin.Services;
using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IUserAdminService _service;

    public UsersController(IUserAdminService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string? role, bool? isActive, string? keyword)
    {
        var model = await _service.GetIndexAsync(role, isActive, keyword);
        model.FilterRole = role;
        model.FilterIsActive = isActive;
        model.SearchKeyword = keyword;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserAdminDetailViewModel vm, string action)
    {
        if (action == "toggleActive")
        {
            await _service.ToggleActiveAsync(vm.Id);
            return RedirectToAction(nameof(Index));
        }

        if (action == "updateRole")
        {
            await _service.UpdateRoleAsync(vm.Id, vm.Role);
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}