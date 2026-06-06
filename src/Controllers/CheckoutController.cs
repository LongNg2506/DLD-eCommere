using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MyProject.Data;
using MyProject.Models;
using MyProject.Services.Cart;
using MyProject.Services.Order;
using MyProject.Services.Payment;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace MyProject.Controllers;

public class CheckoutController : Controller
{
	private readonly ICartService _cartService;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly AppDbContext _db;
	private readonly IConfiguration _configuration;

	public CheckoutController(ICartService cartService, IOrderService orderService, IPaymentService paymentService, AppDbContext db, IConfiguration configuration)
	{
		_cartService = cartService;
		_orderService = orderService;
		_paymentService = paymentService;
		_db = db;
		_configuration = configuration;
	}

	private string GetSessionId() => HttpContext.Session.Id;
	private int? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value != null
	? int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value) : null;

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var cart = await _cartService.GetCartAsync(GetSessionId(), GetUserId());
		if (!cart.Items.Any()) return RedirectToAction("Index", "Cart");
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

		if (model.PaymentMethod == "OnlinePayment")
		{
			return RedirectToAction(nameof(OnlinePaymentQR), new { orderId = result.OrderId });
		}

		return RedirectToAction(nameof(Confirmation), new { orderId = result.OrderId });
	}

	[HttpGet]
	public async Task<IActionResult> OnlinePaymentQR(int orderId)
	{
		var order = await _db.Orders.FindAsync(orderId);
		if (order == null) return NotFound();

		var bank = await _db.BankAccounts.FirstOrDefaultAsync(b => b.IsActive);
		if (bank == null) return RedirectToAction(nameof(PaymentSuccess), new { orderId });

		var baseUrl = _configuration["AppSettings:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
		var confirmUrl = $"{baseUrl}/Checkout/ConfirmOnlinePayment?orderId={orderId}";

		string qrBase64;
		using (var qrGenerator = new QRCodeGenerator())
		{
			var qrData = qrGenerator.CreateQrCode(confirmUrl, QRCodeGenerator.ECCLevel.M);
			using var qrCode = new Base64QRCode(qrData);
			qrBase64 = qrCode.GetGraphic(20);
		}

		var model = new OnlinePaymentQRViewModel
		{
			OrderId = orderId,
			InvoiceNumber = order.InvoiceNumber ?? $"ORD{orderId:D5}",
			Amount = order.TotalAmount,
			BankName = bank.BankName,
			AccountName = bank.AccountName,
			AccountNumber = bank.AccountNumber,
			Branch = bank.Branch,
			TransferContent = $"DLD {order.InvoiceNumber ?? orderId.ToString()}",
			QRCodeBase64 = qrBase64
		};

		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> MockOnlinePaymentConfirmed(int orderId)
	{
		var order = await _db.Orders.FindAsync(orderId);
		if (order == null) return NotFound();

		var bank = await _db.BankAccounts.FirstOrDefaultAsync(b => b.IsActive);
		var transactionId = bank != null ? $"BT-{bank.AccountNumber.Substring(0, 4)}-{DateTime.Now:yyyyMMddHHmmss}" : $"BT-{orderId}-{DateTime.Now:yyyyMMddHHmmss}";

		await _paymentService.ConfirmPaymentAsync(orderId, transactionId, "Mock online payment confirmed");
		return RedirectToAction(nameof(PaymentSuccess), new { orderId });
	}

	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> ConfirmOnlinePayment(int orderId)
	{
		var order = await _db.Orders.FindAsync(orderId);
		if (order == null) return NotFound();

		ViewBag.OrderId = orderId;
		ViewBag.InvoiceNumber = order.InvoiceNumber ?? $"ORD{orderId:D5}";
		ViewBag.Amount = order.TotalAmount;
		return View();
	}

	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> CheckPaymentStatus(int orderId)
	{
		var order = await _db.Orders.FindAsync(orderId);
		if (order == null) return Json(new { status = "NotFound" });

		return Json(new { status = order.PaymentStatus });
	}

	[HttpPost]
	[AllowAnonymous]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> ConfirmOnlinePayment(int orderId, string? note)
	{
		var order = await _db.Orders.FindAsync(orderId);
		if (order == null) return NotFound();

		var bank = await _db.BankAccounts.FirstOrDefaultAsync(b => b.IsActive);
		var transactionId = bank != null
			? $"QR-{bank.AccountNumber.Substring(0, 4)}-{DateTime.Now:yyyyMMddHHmmss}"
			: $"QR-{orderId}-{DateTime.Now:yyyyMMddHHmmss}";

		await _paymentService.ConfirmPaymentAsync(orderId, transactionId, note ?? "QR payment confirmed by customer");
		return RedirectToAction(nameof(PaymentSuccess), new { orderId });
	}

	
	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> PaymentSuccess(int orderId)
	{
		var order = await _db.Orders.FindAsync(orderId);
		if (order == null) return NotFound();

		ViewBag.OrderId = orderId;
		ViewBag.InvoiceNumber = order.InvoiceNumber ?? "ORD" + orderId.ToString().PadLeft(5, '0');
		ViewBag.Amount = order.TotalAmount;
		return View("PaymentSuccess");
	}

	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> Confirmation(int orderId)
	{
		OrderDetailDto? order = null;
		var userId = GetUserId();
		if (userId.HasValue)
		{
			order = await _orderService.GetOrderAsync(orderId, userId.Value);
		}
		else
		{
			var entity = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (entity != null)
			{
				order = new OrderDetailDto
				{
					Id = entity.Id,
					InvoiceNumber = entity.InvoiceNumber ?? string.Empty,
					Status = entity.Status,
					PaymentMethod = entity.PaymentMethod,
					PaymentStatus = entity.PaymentStatus,
					CustomerName = entity.CustomerName,
					Phone = entity.Phone,
					ShippingAddress = entity.ShippingAddress,
					Note = entity.Note,
					OrderDate = entity.OrderDate,
					TotalAmount = entity.TotalAmount,
					Items = new List<OrderItemDto>(),
					StatusHistory = new List<OrderStatusHistoryDto>()
				};
			}
		}

		if (order == null) return NotFound();
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

public class OnlinePaymentQRViewModel
{
	public int OrderId { get; set; }
	public string InvoiceNumber { get; set; } = string.Empty;
	public decimal Amount { get; set; }
	public string BankName { get; set; } = string.Empty;
	public string AccountName { get; set; } = string.Empty;
	public string AccountNumber { get; set; } = string.Empty;
	public string? Branch { get; set; }
	public string TransferContent { get; set; } = string.Empty;
	public string QRCodeBase64 { get; set; } = string.Empty;
}
