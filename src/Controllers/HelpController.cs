using Microsoft.AspNetCore.Mvc;

namespace MyProject.Controllers;

public class HelpController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
