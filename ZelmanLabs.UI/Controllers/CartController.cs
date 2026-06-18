using Microsoft.AspNetCore.Mvc;
using CarShop.Domain.Modeles;
using ZelmanLabs.UI.Extensions;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Controllers;

public class CartController : Controller
{
  private readonly ICarService _carService;

  public CartController(ICarService carService)
  {
    _carService = carService;
  }

  /// <summary>
  /// Просмотр корзины
  /// </summary>
  public IActionResult Index()
  {
    var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
    return View(cart.CartItems);
  }

  /// <summary>
  /// Добавление автомобиля в корзину
  /// </summary>
  /// <param name="id">Идентификатор автомобиля</param>
  /// <param name="returnUrl">URL для возврата</param>
  [Route("[controller]/add/{id:int}")]
  public async Task<ActionResult> Add(int id, string returnUrl)
  {
    var data = await _carService.GetCarByIdAsync(id);
    if (data.Success && data.Data != null)
    {
      var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
      cart.AddToCart(data.Data);
      HttpContext.Session.Set<Cart>("cart", cart);
    }
    return Redirect(returnUrl);
  }

  /// <summary>
  /// Удаление автомобиля из корзины
  /// </summary>
  /// <param name="id">Идентификатор автомобиля</param>
  [Route("[controller]/remove/{id:int}")]
  public IActionResult Remove(int id)
  {
    var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
    cart.RemoveAll(id);
    HttpContext.Session.Set<Cart>("cart", cart);
    return RedirectToAction("Index");
  }

  /// <summary>
  /// Очистка корзины
  /// </summary>
  [Route("[controller]/clear")]
  public IActionResult Clear()
  {
    HttpContext.Session.Remove("cart");
    return RedirectToAction("Index");
  }
}