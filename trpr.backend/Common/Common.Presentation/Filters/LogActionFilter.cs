using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Modules.Users.Presentation.Fitlers
{
    public class LogActionFilter(ILogger<LogActionFilter> logger) : IAsyncActionFilter, IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger = logger;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            if (context.Exception == null)
            {
                _logger.LogInformation("Action {ActionName} completed successfully", actionName);
            }
            else
            {
                _logger.LogError(context.Exception, "Action {ActionName} failed", actionName);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var arguments = context.ActionArguments.ToDictionary(kv => kv.Key, kv => kv.Value);
            _logger.LogInformation("Starting action {ActionName} with args {@Args}", actionName, arguments);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var arguments = context.ActionArguments.ToDictionary(kv => kv.Key, kv => kv.Value);
            _logger.LogInformation("Starting action {ActionName} with args {@Args}", actionName, arguments);

            var executedContext = await next();

            if (executedContext.Exception == null)
            {
                _logger.LogInformation("Action {ActionName} completed successfully", actionName);
            }
            else
            {
                _logger.LogError(executedContext.Exception, "Action {ActionName} failed", actionName);
            }
        }
    }
}