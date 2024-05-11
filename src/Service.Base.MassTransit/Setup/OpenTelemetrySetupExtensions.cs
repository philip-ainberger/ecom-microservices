using OpenTelemetry.Trace;

namespace Service.Base.MassTransit.Setup;

public static class OpenTelemetrySetupExtensions
{
    public static TracerProviderBuilder AddMassTransitTracing(this TracerProviderBuilder builder)
    {
        return builder.AddSource("MassTransit");
    }
}