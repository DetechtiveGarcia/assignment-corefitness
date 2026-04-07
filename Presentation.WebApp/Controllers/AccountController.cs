using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;

namespace Presentation.WebApp.Controllers
{
    public class AccountController : Controller
    {
        [HideInMenu]
        public IActionResult Index()
        {
            return View();
        }
    }
}
