using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using OperationContext.Core;

namespace OperationContext.Hangfire;

public class OperationContextFilterAttribute(IOperationContext operationContext)
    : JobFilterAttribute,
        IClientFilter,
        IServerFilter
{
    private const string ParameterPrefix = "OperationContext:";

    public void OnCreating(CreatingContext context)
    {
        var items = operationContext.Items;
        if (items != null)
        {
            foreach (var item in items)
            {
                string key = item.Key;
                context.SetJobParameter(GetParameterName(key), item.Value);
            }
        }
    }

    public void OnCreated(CreatedContext context) { }

    public void OnPerforming(PerformingContext context)
    {
        var items = operationContext.Items ??= new Dictionary<string, string>();

        foreach (var paramterName in context.BackgroundJob.ParametersSnapshot.Keys)
        {
            if (IsOperationContextParameter(paramterName))
            {
                items.TryAdd(GetKey(paramterName), context.GetJobParameter<string>(paramterName));
            }
        }
    }

    public void OnPerformed(PerformedContext context)
    {
        operationContext.Items = null;
    }

    private static bool IsOperationContextParameter(string paramterName) =>
        paramterName.StartsWith(ParameterPrefix, StringComparison.OrdinalIgnoreCase);

    private static string GetParameterName(string key) => ParameterPrefix + key;

    private static string GetKey(string parameterName) =>
        parameterName.Replace(ParameterPrefix, string.Empty, StringComparison.OrdinalIgnoreCase);
}
