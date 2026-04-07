using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Attributes.MenuNavigation;
using Presentation.WebApp.Models.Forms;

namespace Presentation.WebApp.Controllers
{
    public class CustomerServiceController : Controller
    {
        [MenuItem(title: "Customer Service", 4)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactForm form)
        {
            if (!ModelState.IsValid)
                return View(form);

            return RedirectToAction(nameof(Index));
        }
    }
}
