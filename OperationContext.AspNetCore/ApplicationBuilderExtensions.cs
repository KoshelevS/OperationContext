using Microsoft.AspNetCore.Builder;

namespace OperationContext.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseOperationContext(
        this IApplicationBuilder app,
        Action<OperationContextConfig>? action = null
    )
    {
        var cfg = new OperationContextConfig();
        action?.Invoke(cfg);

        return app.UseOperationContext(cfg);
    }

    public static IApplicationBuilder UseOperationContext(
        this IApplicationBuilder app,
        OperationContextConfig? cfg = null
    )
    {
        cfg ??= new OperationContextConfig();

        app.UseMiddleware<OperationContextMiddleware>();

        if (cfg.AddCorrelationId)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
        }

        if (cfg.CreateLoggerScope)
        {
            return app.UseMiddleware<OperationContextLoggerMiddleware>();
        }

        return app;
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseOperationContextLogger(this IApplicationBuilder app)
    {
        app.UseMiddleware<OperationContextLoggerMiddleware>();
        return app;
    }
}
