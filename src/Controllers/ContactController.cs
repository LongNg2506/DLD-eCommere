using Microsoft.AspNetCore.Mvc;

namespace MyProject.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
