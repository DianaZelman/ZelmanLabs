using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ZelmanLabs.UI.Extensions;

/// <summary>
/// Методы расширения для работы с сессией
/// </summary>
public static class SessionExtensions
{
  /// <summary>
  /// Сохранить объект в сессию
  /// </summary>
  /// <typeparam name="T">Тип объекта</typeparam>
  /// <param name="session">Сессия</param>
  /// <param name="key">Ключ</param>
  /// <param name="value">Значение</param>
  public static void Set<T>(this ISession session, string key, T value)
  {
    var json = JsonSerializer.Serialize(value);
    session.SetString(key, json);
  }

  /// <summary>
  /// Получить объект из сессии
  /// </summary>
  /// <typeparam name="T">Тип объекта</typeparam>
  /// <param name="session">Сессия</param>
  /// <param name="key">Ключ</param>
  /// <returns>Объект или null</returns>
  public static T? Get<T>(this ISession session, string key)
  {
    var json = session.GetString(key);
    if (string.IsNullOrEmpty(json))
    {
      return default;
    }
    return JsonSerializer.Deserialize<T>(json);
  }
}