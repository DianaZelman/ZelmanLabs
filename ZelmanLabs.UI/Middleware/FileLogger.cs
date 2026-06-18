using Serilog;

namespace ZelmanLabs.UI.Middleware;

/// <summary>
/// Компонент промежуточного ПО для логирования запросов с кодом != 2XX
/// </summary>
public class FileLogger
{
  private readonly RequestDelegate _next;

  public FileLogger(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext httpContext)
  {
    await _next(httpContext);

    var code = httpContext.Response.StatusCode;
    var codeGroup = code / 100;

    if (codeGroup != 2)
    {
      Log.Logger.Information($"--> Request {httpContext.Request.Path} returns {code}");
    }
  }
}