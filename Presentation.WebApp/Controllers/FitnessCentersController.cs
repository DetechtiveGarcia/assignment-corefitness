using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class FitnessCentersController : Controller
{
    [MenuItem(title: "Fitness Centers", 1)]
    public IActionResult Index()
    {
        return View();
    }
}
