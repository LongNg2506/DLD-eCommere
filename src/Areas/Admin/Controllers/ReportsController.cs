using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Areas.Admin.Services;

namespace MyProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReportsController : Controller
{
    private readonly IReportAdminService _service;

    public ReportsController(IReportAdminService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _service.GetReportAsync();
        return View(model);
    }
}