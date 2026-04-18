using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class StoreController : Controller
{
    [MenuItem(title: "Store", order: 5)]
    public IActionResult Index()
    {
        return View();
    }
}
