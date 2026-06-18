using CarShop.Domain.Entities;

namespace CarShop.Domain.Modeles;

/// <summary>
/// Элемент корзины заказов
/// </summary>
public class CartItem
{
  /// <summary>
  /// Объект (автомобиль)
  /// </summary>
  public Car Item { get; set; } = null!;

  /// <summary>
  /// Количество
  /// </summary>
  public int Qty { get; set; }
}