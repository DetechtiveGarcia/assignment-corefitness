using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers;

public class TrainingController : Controller
{
    [MenuItem(title: "Personal Training", order: 3)]
    public IActionResult PersonalTraining()
    {
        return View();
    }

    [MenuItem(title: "Online Coaching", 2)]
    public IActionResult OnlineCoaching()
    {
        return View();
    }

    [MenuItem(title: "Group Training", order: 3)]
    public IActionResult GroupTraining()
    {
        return View();
    }


}
