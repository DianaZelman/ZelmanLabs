using Microsoft.AspNetCore.Mvc;

namespace ZelmanLabs.UI.ViewComponents
{
  public class CartViewComponent : ViewComponent
  {
    public IViewComponentResult Invoke()
    {
      //TODO: add logic for cart
      ViewData["total"] = 100.5;

      return View();
    }
  }
}