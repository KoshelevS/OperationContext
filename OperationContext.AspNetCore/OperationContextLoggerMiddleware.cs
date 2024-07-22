using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OperationContext.Core;

namespace OperationContext.AspNetCore;

public class OperationContextLoggerMiddleware(
    RequestDelegate next,
    IOperationContext operationContext,
    ILogger<OperationContextLoggerMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var items = operationContext.Items;
        if (items != null && logger.IsEnabled(LogLevel.Information))
        {
            using var scope = logger.BeginScope(new OperationContextLogScope(items));
            await next(context);
        }
        else
        {
            await next(context);
        }
    }
}
