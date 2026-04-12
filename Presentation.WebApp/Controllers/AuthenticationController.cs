using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController : Controller
{
    [HideInMenu]
    [HttpGet("authentication/signup-email")]
    public IActionResult SignUpEmail()
    {
        return View();
    }

    [HideInMenu]
    [HttpGet]
    public IActionResult SignUpPassword()
    {
        var email = HttpContext.Session.GetString("Email");

        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction("SignUpEmail");

        return View();
    }

    [HttpGet]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }


}
