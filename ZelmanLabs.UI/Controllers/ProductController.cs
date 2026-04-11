using Microsoft.AspNetCore.Mvc;

namespace ZelmanLabs.UI.Controllers
{
  public class Product : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}