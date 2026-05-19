using Api.Middleware;

namespace Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder AddMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionLocalizationMiddleware>();
        return app;
    }
}
