using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using OperationContext.Core;

namespace OperationContext.Hangfire;

public class OperationContextLoggerFilterAttribute(
    IOperationContext operationContext,
    ILogger<OperationContextLoggerFilterAttribute> logger
) : JobFilterAttribute, IServerFilter
{
    private static readonly AsyncLocal<IDisposable?> scope = new();

    public void OnPerforming(PerformingContext context)
    {
        var items = operationContext.Items;
        if (items != null && logger.IsEnabled(LogLevel.Information))
        {
            scope.Value = logger.BeginScope(new OperationContextLogScope(items));
        }
    }

    public void OnPerformed(PerformedContext context)
    {
        scope.Value?.Dispose();
        scope.Value = null;
    }
}
