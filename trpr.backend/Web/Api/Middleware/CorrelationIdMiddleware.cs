using Common.Application.Correlation;
using Serilog.Context;

namespace Api.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private const string CorrelationHeader = "X-Correlation-ID";
        public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger, ICorrelationIdAccessor correlationAccessor)
        {
            var correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();
            correlationAccessor.CorrelationId = correlationId;
            logger.LogInformation("Correlation id : {correlationId}", correlationId);
            context.Response.Headers[CorrelationHeader] = correlationId;
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                context.Items["CorrelationId"] = correlationId;
                await _next(context);
            }
        }
    }
}