using Microsoft.AspNetCore.Mvc;

namespace PracticalNCode.Web.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            if (User.IsInRole("Organization"))
            {
                return RedirectToAction("Index", "Organization");
            }

            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Employee");
            }
        }

        return RedirectToAction("Login", "Account");
    }
}
