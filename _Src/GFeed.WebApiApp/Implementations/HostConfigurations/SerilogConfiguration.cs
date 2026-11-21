using Serilog;
using Serilog.Events;

namespace GFeed.WebApiApp.Implementations.HostConfigurations;

public static class SerilogConfigurationExtensions
{
    public static void AddLogConfiguration(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(new Serilog.Formatting.Compact.CompactJsonFormatter())
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
                    ?? "http://localhost:4317";
                options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = builder.Configuration["OTEL_SERVICE_NAME"] ?? "feedback-quiz"
                };
            })
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
