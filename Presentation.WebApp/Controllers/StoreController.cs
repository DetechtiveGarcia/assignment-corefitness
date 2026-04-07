using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class StoreController : Controller
{
    [MenuItem(title: "Store", order: 1)]
    public IActionResult Index()
    {
        return View();
    }
}
