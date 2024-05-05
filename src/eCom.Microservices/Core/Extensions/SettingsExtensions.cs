using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions;

public static class SettingsExtensions
{
    public static void AddCustomSettingsWithValidationOnStart<T>(this IServiceCollection services, string key) where T : class
    {
        services
            .AddOptions<T>()
            .BindConfiguration(key)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}