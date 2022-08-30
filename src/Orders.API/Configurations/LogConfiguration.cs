using Serilog;
using Serilog.Filters;
using Serilog.Exceptions;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog.Events;

namespace Orders.API.Configurations;

public static class LogConfiguration
{
    public static IHostBuilder AddLogger(this IHostBuilder builder)
    {
        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", "Orders.API")
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Hosting.Diagnostics"))
            .Filter.ByExcluding(Matching.FromSource("Serilog.AspNetCore.RequestLoggingMiddleware"))
            .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
            .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
            .MinimumLevel.Override("MassTransit", LogEventLevel.Information)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .CreateLogger();

        builder.UseSerilog(Log.Logger);
        return builder;
    }
}
