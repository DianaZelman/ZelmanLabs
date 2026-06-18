using Microsoft.AspNetCore.Mvc;
using CarShop.Domain.Modeles;
using ZelmanLabs.UI.Extensions;

namespace ZelmanLabs.UI.ViewComponents;

/// <summary>
/// Компонент для отображения корзины в меню
/// </summary>
public class CartViewComponent : ViewComponent
{
  public IViewComponentResult Invoke()
  {
    // Получаем корзину из сессии
    var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
    return View(cart);
  }
}