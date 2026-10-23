using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Models;

namespace PracticalNCode.Web.Controllers;

public sealed class AccountController(IAuthService authService) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToRoleDashboard();
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, errorMessage, user) = await authService.ValidateUserAsync(model.Username, model.Password, cancellationToken);
        if (!success || user is null)
        {
            ModelState.AddModelError(string.Empty, errorMessage ?? "Invalid login attempt.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.RoleName ?? string.Empty),
            new("OrganizationId", user.OrganizationId?.ToString() ?? string.Empty),
            new("EmployeeId", user.EmployeeId?.ToString() ?? string.Empty),
            new("DisplayName", user.EmployeeName ?? user.OrganizationName ?? user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5),
            AllowRefresh = true
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToRoleDashboard(user.RoleName);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "You have been logged out successfully.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToRoleDashboard(string? roleName = null)
    {
        var role = roleName ?? (User.IsInRole("Admin") ? "Admin" : User.IsInRole("Organization") ? "Organization" : User.IsInRole("Employee") ? "Employee" : null);

        return role switch
        {
            "Admin" => RedirectToAction("Index", "Admin"),
            "Organization" => RedirectToAction("Index", "Organization"),
            "Employee" => RedirectToAction("Index", "Employee"),
            _ => RedirectToAction("Index", "Home")
        };
    }
}

