using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticalNCode.Application.Abstractions.Services;

namespace PracticalNCode.Web.Controllers;

[Authorize(Roles = "Employee")]
public sealed class EmployeeController(IEmployeeService employeeService) : Controller
{
    private int? GetEmployeeId()
    {
        var claim = User.FindFirst("EmployeeId")?.Value;
        if (int.TryParse(claim, out var empId) && empId > 0)
        {
            return empId;
        }
        return null;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var empId = GetEmployeeId();
        if (!empId.HasValue)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ErrorMessage"] = "No employee record is associated with your account.";
            return RedirectToAction("Login", "Account");
        }

        var profile = await employeeService.GetEmployeeProfileAsync(empId.Value, cancellationToken);
        if (profile is null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ErrorMessage"] = "Your employee record has been deleted or deactivated. Access is denied.";
            return RedirectToAction("Login", "Account");
        }

        return View(profile);
    }
}

