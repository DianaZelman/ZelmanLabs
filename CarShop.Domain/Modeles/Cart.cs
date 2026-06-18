using CarShop.Domain.Entities;

namespace CarShop.Domain.Modeles;

/// <summary>
/// Корзина заказов
/// </summary>
public class Cart
{
  /// <summary>
  /// Идентификатор корзины
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Список объектов в корзине
  /// key - идентификатор автомобиля
  /// </summary>
  public Dictionary<int, CartItem> CartItems { get; set; } = new();

  /// <summary>
  /// Добавить объект в корзину
  /// </summary>
  /// <param name="car">Добавляемый автомобиль</param>
  public virtual void AddToCart(Car car)
  {
    if (CartItems.ContainsKey(car.Id))
    {
      // Если такой объект уже есть - увеличиваем количество
      CartItems[car.Id].Qty++;
    }
    else
    {
      // Добавляем новый объект
      CartItems.Add(car.Id, new CartItem
      {
        Item = car,
        Qty = 1
      });
    }
  }

  /// <summary>
  /// Удалить объект из корзины (уменьшить количество на 1)
  /// </summary>
  /// <param name="id">Идентификатор автомобиля</param>
  public virtual void RemoveFromCart(int id)
  {
    if (CartItems.ContainsKey(id))
    {
      CartItems[id].Qty--;

      // Если количество стало 0 - удаляем из корзины
      if (CartItems[id].Qty <= 0)
      {
        CartItems.Remove(id);
      }
    }
  }

  /// <summary>
  /// Удалить все объекты из корзины
  /// </summary>
  /// <param name="id">Идентификатор автомобиля</param>
  public virtual void RemoveAll(int id)
  {
    if (CartItems.ContainsKey(id))
    {
      CartItems.Remove(id);
    }
  }

  /// <summary>
  /// Полное количество объектов в корзине
  /// </summary>
  public int Count
  {
    get
    {
      int total = 0;
      foreach (var item in CartItems.Values)
      {
        total += item.Qty;
      }
      return total;
    }
  }

  /// <summary>
  /// Очистить корзину
  /// </summary>
  public virtual void Clear()
  {
    CartItems.Clear();
  }
}