using System.Text.Json;
using Common.Domain;

namespace Api.Middleware;

public class ExceptionLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLocalizationMiddleware> _logger;

    public ExceptionLocalizationMiddleware(RequestDelegate next, ILogger<ExceptionLocalizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (LocalizedHttpException ex)
        {
            var key = $"{ex.ErrorCode}_{ex.StatusCode}";
            var localizedMessage = key + " " + string.Join(", ", ex.MessageArgs);

            _logger.LogWarning(ex,
                "Localized exception occurred. Path: {Path}, Method: {Method}, StatusCode: {StatusCode}, ErrorCode: {ErrorCode}, Args: {Args}",
                context.Request.Path,
                context.Request.Method,
                ex.StatusCode,
                ex.ErrorCode,
                string.Join(", ", ex.MessageArgs)
            );

            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            string[] args = ex.MessageArgs.Where(arg => arg != null).Select(arg => arg.ToString()!).ToArray();
            await context.Response.WriteAsJsonAsync(new
            {
                code = ex.ErrorCode,
                status = ex.StatusCode,
                args = JsonSerializer.Serialize<string[]>(args)
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Unhandled exception occurred. Path: {Path}, Method: {Method}",
                context.Request.Path,
                context.Request.Method
            );

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                code = "INTERNAL_ERROR",
                status = 500,
                args = "[]"
            });
        }
    }
}