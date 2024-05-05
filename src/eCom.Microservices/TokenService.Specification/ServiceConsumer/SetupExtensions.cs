using Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace TokenService.Specification.ServiceConsumer;

public static class SetupExtensions
{
    public static IServiceCollection AddTokenServiceForConsumer(this IServiceCollection services)
    {
        services.AddCustomSettingsWithValidationOnStart<TokenServiceEndpointSettings>("TokenServiceEndpoint");
        services.AddScoped<ITokenClientService, TokenClientService>();

        return services;
    }
}