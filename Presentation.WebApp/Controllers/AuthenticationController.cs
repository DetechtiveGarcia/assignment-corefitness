using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController : Controller
{
    [HideInMenu]
    public IActionResult SignUp()
    {
        return View();
    }

    [HideInMenu]
    public IActionResult SetPassword()
    {
        return View();
    }

    [HideInMenu]
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }
}
