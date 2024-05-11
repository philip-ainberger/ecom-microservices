using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Service.Base.Settings;

namespace Service.Base.Setup;

public static class OpenTelemetrySetupExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="applicationName">Use WebApplicationBuilder.Environment.ApplicationName</param>
    /// <returns></returns>
    public static IOpenTelemetryBuilder AddCurrentServiceAsResource(this IOpenTelemetryBuilder builder, string applicationName)
    {
        return builder.ConfigureResource(resource => resource.AddService(serviceName: applicationName));
    }

    public static OtlpExportProtocol TranslateOtelExportProtocol(OpenTelemetryExportProtocol protocol)
        => protocol switch
        {
            OpenTelemetryExportProtocol.HTTP => OtlpExportProtocol.HttpProtobuf,
            OpenTelemetryExportProtocol.GRPC => OtlpExportProtocol.Grpc,
            _ => throw new NotImplementedException()
        };

    public static TracerProviderBuilder AddDefaultTracing(this TracerProviderBuilder builder, IHostApplicationBuilder hostApplication)
    {
        var settings = hostApplication.GetSettings();

        builder
            .AddSource(hostApplication.GetApplicationName())
            //.SetSampler(new TraceIdRatioBasedSampler(1.0))
            .AddAspNetCoreInstrumentation()
            //.AddHttpClientInstrumentation()
            .AddOtlpExporter(conf =>
            {
                conf.Endpoint = new Uri(settings.GlobalExportEndpoint + "/v1/traces");
                conf.Protocol = TranslateOtelExportProtocol(settings.GlobalExportProtocol);
            })
            .AddConsoleExporter();

        return builder;
    }

    public static IOpenTelemetryBuilder AddTracing(this IOpenTelemetryBuilder builder, Action<TracerProviderBuilder> configure)
    {
        return builder.WithTracing(configure.Invoke);
    }

    public static IOpenTelemetryBuilder AddMetrics(this IOpenTelemetryBuilder builder, Action<MeterProviderBuilder> configure)
    {
        return builder.WithMetrics(configure.Invoke);
    }

    public static MeterProviderBuilder AddDefaultMetrics(this MeterProviderBuilder builder, IHostApplicationBuilder hostApplication)
    {
        var settings = hostApplication.GetSettings();

        builder.AddAspNetCoreInstrumentation()
            .AddMeter(hostApplication.GetApplicationName())
            .AddMeter("Microsoft.AspNetCore.Hosting")
            .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
            .AddOtlpExporter((conf, metricReaderOptions) =>
            {
                conf.Endpoint = new Uri(settings.GlobalExportEndpoint);
                conf.Protocol = TranslateOtelExportProtocol(settings.GlobalExportProtocol);

                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 4000;
            });

        return builder;
    }
        
    public static OpenTelemetryLoggerOptions AddDefaultLogging(this OpenTelemetryLoggerOptions builder, IHostApplicationBuilder hostApplication)
    {
        var settings = hostApplication.GetSettings();

        builder.IncludeFormattedMessage = true;

        builder.AddOtlpExporter(conf =>
        {
            conf.Endpoint = new Uri(settings.GlobalExportEndpoint);
            conf.Protocol = TranslateOtelExportProtocol(settings.GlobalExportProtocol);
        });

        return builder;
    }

    public static OpenTelemetrySettings GetSettings(this IHostApplicationBuilder hostApplication)
    {
        return hostApplication.Services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<OpenTelemetrySettings>>().Value;
    }
}