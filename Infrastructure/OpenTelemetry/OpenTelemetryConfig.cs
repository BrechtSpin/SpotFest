using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Exporter;
using MassTransit.Logging;
using Contracts;

namespace Infrastructure.OpenTelemetry;

public static class OpenTelemetryService
{
    private static readonly string _serviceName = Assembly.GetEntryAssembly()?.GetName().Name!;
    public static IServiceCollection AddOpenTelemetryService(
    this IServiceCollection services,
        string otlpEndpoint = "http://localhost:4317")
    {
        services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService(_serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddSource(BaggageKeys.UserGuid)                 // custom baggagekey
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource(DiagnosticHeaders.DefaultListenerName) // MassTransit ActivitySource
                    .AddMassTransitInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otlpEndpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics
                    //.AddProcessInstrumentation()        // OS/platform/container                                                        
                    //.AddRuntimeInstrumentation()        // CLR of this service
                    .AddAspNetCoreInstrumentation()     // inbound API calls
                    .AddHttpClientInstrumentation()     // outbound http calls
                    .AddMeter("MassTransit")
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otlpEndpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    });
            });
        services.AddLogging(logging =>
        {
            logging.AddOpenTelemetry(OptlLog =>
            {
                OptlLog.IncludeScopes = true;
                OptlLog.ParseStateValues = true;
                OptlLog.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(otlpEndpoint);
                    options.Protocol = OtlpExportProtocol.Grpc;
                });
            });
        });
        return services;
    }
}
