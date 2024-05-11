using Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Service.Base.Settings;

namespace Service.Base.Setup;

public static class SettingsExtensions
{
    public static IServiceCollection AddOpenTelemetrySettings(this IServiceCollection services)
    {
        services.AddCustomSettingsWithValidationOnStart<OpenTelemetrySettings>("OpenTelemetry");

        return services;
    }
}