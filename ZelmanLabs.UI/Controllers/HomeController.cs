using Microsoft.AspNetCore.Mvc;

namespace ZelmanLabs.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
