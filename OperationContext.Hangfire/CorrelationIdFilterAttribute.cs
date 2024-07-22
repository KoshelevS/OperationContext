using Hangfire.Common;
using Hangfire.Server;
using OperationContext.Core;

namespace OperationContext.Hangfire;

public class CorrelationIdFilterAttribute(IOperationContext operationContext)
    : JobFilterAttribute,
        IServerFilter
{
    public void OnPerforming(PerformingContext context)
    {
        operationContext.Items?.TryAdd(
            OperationContextKeys.CorrelationId,
            context.BackgroundJob.Id
        );
    }

    public void OnPerformed(PerformedContext context) { }
}
