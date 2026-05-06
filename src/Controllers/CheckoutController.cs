using Microsoft.AspNetCore.Mvc;
using MyProject.Services.Cart;
using MyProject.Services.Order;

namespace MyProject.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public CheckoutController(ICartService cartService, IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    private string GetSessionId() => HttpContext.Session.Id;
    private int? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value != null
        ? int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value) : null;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
        if (!cart.Items.Any())
            return RedirectToAction("Index", "Cart");

        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
            return View("Index", cart);
        }

        var stockValid = await _cartService.ValidateStockAsync(GetSessionId(), GetUserId());
        if (!stockValid)
        {
            ModelState.AddModelError("", "Some items in your cart are out of stock. Please adjust your order.");
            var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
            return View("Index", cart);
        }

        var request = new CreateOrderRequest
        {
            UserId = GetUserId(),
            SessionId = GetSessionId(),
            CustomerName = model.CustomerName,
            Phone = model.Phone,
            ShippingAddress = model.ShippingAddress,
            PaymentMethod = model.PaymentMethod,
            Note = model.Note
        };

        var result = await _orderService.CreateOrderAsync(request);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
            return View("Index", cart);
        }

        return RedirectToAction(nameof(Confirmation), new { orderId = result.OrderId });
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(int orderId)
    {
        var order = await _orderService.GetOrderAsync(orderId, GetUserId());
        if (order == null)
            return NotFound();

        return View(order);
    }
}

public class CheckoutViewModel
{
    public string CustomerName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "COD";
    public string? Note { get; set; }
}
