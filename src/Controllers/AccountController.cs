using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.DTOs.Users;
using MyProject.Services.Auth;
using MyProject.Services.Cart;

namespace MyProject.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ICartService _cartService;

    public AccountController(IAuthService authService, ICartService cartService)
    {
        _authService = authService;
        _cartService = cartService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError("", "Email is required.");
            return View();
        }

        // Trong thực tế, đây là nơi gọi service để gửi email reset password
        // Hiện tại chúng ta sẽ giả lập là đã gửi email thành công
        ViewBag.Message = "If an account with this email exists, a password reset link has been sent.";
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var user = await _authService.LoginAsync(dto);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(dto);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "ChronoAuth");
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = dto.RememberMe,
            ExpiresUtc = dto.RememberMe
                ? DateTimeOffset.UtcNow.AddDays(14)
                : DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync("ChronoAuth", principal, authProperties);

        // Merge guest cart with user cart after login
        var guestSessionId = HttpContext.Session.Id;
        await _cartService.MergeCartAsync(guestSessionId, user.Id);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        if (await _authService.EmailExistsAsync(dto.Email))
        {
            ModelState.AddModelError("Email", "This email is already registered.");
            return View(dto);
        }

        var user = await _authService.RegisterAsync(dto);

        if (user == null)
        {
            ModelState.AddModelError("", "Unable to create account. Please try again.");
            return View(dto);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "ChronoAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("ChronoAuth", principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("ChronoAuth");
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> DebugUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdClaim, out var userId);
        var orders = await _authService.GetOrdersByUserIdAsync(userId);
        return Json(new
        {
            claimValue = userIdClaim,
            parsedUserId = userId,
            isAuthenticated = User.Identity?.IsAuthenticated,
            userName = User.Identity?.Name,
            ordersCount = orders.Count,
            orders = orders
        });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Profile(string tab = "profile")
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login");

        var user = await _authService.GetByIdAsync(userId);
        if (user == null)
            return RedirectToAction("Logout");

        var orders = await _authService.GetOrdersByUserIdAsync(userId);

        ViewData["ActiveTab"] = tab;
        ViewData["Orders"] = orders;

        return View(user);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(UserDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login");

        if (!ModelState.IsValid)
        {
            var current = await _authService.GetByIdAsync(userId);
            return View("Profile", current);
        }

        var updated = await _authService.UpdateProfileAsync(userId, dto.FullName, dto.Phone, dto.Address);
        if (updated == null)
            return RedirectToAction("Logout");

        return RedirectToAction("Profile", new { tab = "profile" });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login");

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
        {
            TempData["PasswordError"] = "New passwords do not match.";
            return RedirectToAction("Profile", new { tab = "password" });
        }

        if (newPassword.Length < 6)
        {
            TempData["PasswordError"] = "Password must be at least 6 characters.";
            return RedirectToAction("Profile", new { tab = "password" });
        }

        var success = await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);
        if (!success)
        {
            TempData["PasswordError"] = "Current password is incorrect.";
            return RedirectToAction("Profile", new { tab = "password" });
        }

        TempData["PasswordSuccess"] = "Password changed successfully.";
        return RedirectToAction("Profile", new { tab = "password" });
    }
}
