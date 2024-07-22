using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OperationContext.Core;

namespace OperationContext.Hangfire;

public static class GlobalConfigurationExtensions
{
    public static IGlobalConfiguration UseOperationContext(
        this IGlobalConfiguration config,
        IServiceProvider serviceProvider,
        Action<OperationContextConfig>? action = null
    )
    {
        var cfg = new OperationContextConfig();
        action?.Invoke(cfg);

        return config.UseOperationContext(serviceProvider, cfg);
    }

    public static IGlobalConfiguration UseOperationContext(
        this IGlobalConfiguration config,
        IServiceProvider serviceProvider,
        OperationContextConfig? cfg = null
    )
    {
        cfg ??= new OperationContextConfig();

        var operationContext = serviceProvider.GetRequiredService<IOperationContext>();
        config.UseFilter(new OperationContextFilterAttribute(operationContext));

        if (cfg.AddCorrelationId)
        {
            config.UseFilter(new CorrelationIdFilterAttribute(operationContext));
        }

        if (cfg.CreateLoggerScope)
        {
            var logger = serviceProvider.GetRequiredService<
                ILogger<OperationContextLoggerFilterAttribute>
            >();
            config.UseFilter(new OperationContextLoggerFilterAttribute(operationContext, logger));
        }

        return config;
    }

    public static IGlobalConfiguration UseCorrelationId(
        this IGlobalConfiguration config,
        IServiceProvider serviceProvider
    )
    {
        var operationContext = serviceProvider.GetRequiredService<IOperationContext>();
        config.UseFilter(new CorrelationIdFilterAttribute(operationContext));

        return config;
    }

    public static IGlobalConfiguration UseOperationContextLogger(
        this IGlobalConfiguration config,
        IServiceProvider serviceProvider
    )
    {
        var operationContext = serviceProvider.GetRequiredService<IOperationContext>();
        var logger = serviceProvider.GetRequiredService<
            ILogger<OperationContextLoggerFilterAttribute>
        >();
        config.UseFilter(new OperationContextLoggerFilterAttribute(operationContext, logger));

        return config;
    }
}
