using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class MembershipsController : Controller
{
    [MenuItem(title: "Memberships", order: 2)]
    public IActionResult Index()
    {
        return View();
    }
}
