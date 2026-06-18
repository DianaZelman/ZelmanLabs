namespace ZelmanLabs.UI.Middleware;

/// <summary>
/// Методы расширения для регистрации FileLogger
/// </summary>
public static class FileLoggerExtensions
{
  public static IApplicationBuilder UseFileLogger(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<FileLogger>();
  }
}