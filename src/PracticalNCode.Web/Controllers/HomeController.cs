using Microsoft.AspNetCore.Mvc;
namespace PracticalNCode.Web.Controllers;
public sealed class HomeController : Controller
{
    public IActionResult Index() => View();
}
