using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Areas.Admin.Services;

namespace MyProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class OrdersController : Controller
{
    private readonly IOrderAdminService _service;

    public OrdersController(IOrderAdminService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string? status, string? keyword)
    {
        var model = await _service.GetIndexAsync(status, keyword);
        model.FilterStatus = status;
        model.SearchKeyword = keyword;
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _service.GetDetailAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status, string? note)
    {
        var updatedBy = User.Identity?.Name ?? "Admin";
        await _service.UpdateStatusAsync(id, status, updatedBy, note);
        TempData["Success"] = $"Order status updated to {status}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmPayment(int id, string? transactionId, string? note)
    {
        var updatedBy = User.Identity?.Name ?? "Staff";
        var success = await _service.ConfirmPaymentAsync(id, transactionId, note, updatedBy);
        if (success)
            TempData["Success"] = "Payment confirmed successfully.";
        else
            TempData["Error"] = "Failed to confirm payment.";
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Invoice(int id)
    {
        var model = await _service.GetDetailAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }
}
