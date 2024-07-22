using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OperationContext.Core;

namespace OperationContext.AspNetCore;

public class CorrelationIdMiddleware(
    RequestDelegate next,
    IOperationContext operationContext,
    IOptions<OperationContextOptions> options
)
{
    private const string DefaultHttpHeaderName = "X-Correlation-ID";

    public Task InvokeAsync(HttpContext context)
    {
        operationContext.Items?.TryAdd(
            OperationContextKeys.CorrelationId,
            GetCorrelationId(context)
        );

        return next(context);
    }

    private string GetCorrelationId(HttpContext context)
    {
        var headerName = options.Value.CorrelationIdHttpHeaderName ?? DefaultHttpHeaderName;

        string? correlationId = null;

        if (context.Request.Headers.TryGetValue(headerName, out var value))
        {
            correlationId = value.FirstOrDefault();
        }

        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId = context.TraceIdentifier;
        }

        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        return correlationId;
    }
}
