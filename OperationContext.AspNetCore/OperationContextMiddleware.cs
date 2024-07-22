using Microsoft.AspNetCore.Http;
using OperationContext.Core;

namespace OperationContext.AspNetCore;

public class OperationContextMiddleware(RequestDelegate next, IOperationContext operationContext)
{
    public async Task InvokeAsync(HttpContext context)
    {
        operationContext.Items = new Dictionary<string, string>();

        try
        {
            await next(context);
        }
        finally
        {
            operationContext.Items = null;
        }
    }
}
