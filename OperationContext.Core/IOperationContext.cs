namespace OperationContext.Core;

public interface IOperationContext
{
    IDictionary<string, string>? Items { get; set; }
}
