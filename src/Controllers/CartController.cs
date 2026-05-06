using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Cart;

namespace MyProject.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private string GetSessionId() => HttpContext.Session.Id;
    private int? GetUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (claim == null || !int.TryParse(claim.Value, out var id))
            return null;
        return id;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
        return View(cart);
    }

    [HttpGet]
    public async Task<IActionResult> GetCartPartial()
    {
        var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
        return PartialView("_CartDrawer", cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddItem(int productId, int quantity = 1)
    {
        var result = await _cartService.AddItemAsync(GetSessionId(), GetUserId(), productId, quantity);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(result);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        var result = await _cartService.UpdateQuantityAsync(GetSessionId(), GetUserId(), productId, quantity);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(result);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(int productId)
    {
        var success = await _cartService.RemoveItemAsync(GetSessionId(), GetUserId(), productId);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success, message = success ? "Item removed." : "Item not found." });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(GetSessionId(), GetUserId());
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success = true, message = "Cart cleared." });
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetCartCount()
    {
        var count = await _cartService.GetCartCountAsync(GetSessionId(), GetUserId());
        return Json(new { count });
    }
}
