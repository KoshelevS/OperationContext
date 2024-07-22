using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OperationContext.Core;

namespace OperationContext.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOperationContext(this IServiceCollection services)
    {
        services.TryAddSingleton<IOperationContext, DefaultOperationContext>();
        return services;
    }
}
