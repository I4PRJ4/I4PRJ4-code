using Microsoft.AspNetCore.Mvc;

namespace Projekt_StudieTips.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnauthorizedAccess()
        {
            return View();
        }

    }
}
