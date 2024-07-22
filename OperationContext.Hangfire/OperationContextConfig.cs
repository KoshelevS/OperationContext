namespace OperationContext.Hangfire;

public class OperationContextConfig
{
    public bool AddCorrelationId { get; set; }
    public bool CreateLoggerScope { get; set; }
}
