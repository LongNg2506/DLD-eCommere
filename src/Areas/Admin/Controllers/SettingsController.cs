using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Areas.Admin.Services;
using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    private readonly ISettingsAdminService _service;

    public SettingsController(ISettingsAdminService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var model = _service.GetSettings();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(SettingsAdminViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _service.SaveSettings(vm);
        TempData["Success"] = "Settings saved successfully.";
        return View(vm);
    }
}