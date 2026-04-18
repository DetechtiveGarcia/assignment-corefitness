using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class HomeController : Controller
{

    [HideInMenu]
    public IActionResult Index()
    {
        return View();
    }
}
