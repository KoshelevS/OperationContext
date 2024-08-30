# OperationContext

This package allows arbitrary data to be stored in the context of async operations.

## Usage

### ASP.NET Core

Install NuGet package
```cmd
dotnet add package OperationContext.AspNetCore
```

Open the Program.cs file and configure OperationContext.

Register OperationContext in DI container
```csharp
builder.Services.Configure<OperationContextOptions>(opt =>
    opt.CorrelationIdHttpHeaderName = "X-Correlation-ID"
);
builder.Services.AddOperationContext();
```

Add required ASP.NET Core middlewares
```csharp
app.UseOperationContext(cfg =>
{
    cfg.CreateLoggerScope = true;
    cfg.AddCorrelationId = true;
});
```

Or
```csharp
app.UseOperationContext();
app.UseOperationContextLogger();
app.UseCorrelationId();
```

### Hangfire

Install NuGet package
```cmd
dotnet add package OperationContext.Hangfire
```

Open the Program.cs file and configure OperationContext.

Register OperationContext in DI container
```csharp
builder.Services.AddOperationContext();
```

Add required Hangfire filters
```csharp
services.AddHangfire((provider, cfg) =>
    cfg. ...
        .UseOperationContext(
            provider,
            x =>
            {
                x.CreateLoggerScope = true;
                x.AddCorrelationId = true;
            }
        )
);
```

Or
```
services.AddHangfire((provider, cfg) =>
    cfg. ...
        .UseOperationContext(provider)
        .UseOperationContextLogger(provider)
        .UseCorrelationId(provider)
);
```
