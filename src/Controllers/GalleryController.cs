using Microsoft.AspNetCore.Mvc;

namespace MyProject.Controllers;

public class GalleryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
