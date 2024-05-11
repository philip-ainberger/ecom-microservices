using Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Base.EntityFrameworkCore.Settings;
using Service.Base.Settings;

namespace Service.Base.EntityFrameworkCore.Setup;

public static class EntityFrameworkSetupExtensions
{
    public static IServiceCollection AddDbContextProvider<T>(this IServiceCollection services) where T : DbContext
    {
        return services
            .AddDbContextFactory<T>()
            .AddSingleton<IDbContextProvider<T>, DbContextProvider<T>>();
    }

    public static IServiceCollection AddEntityFrameworkSettings(this IServiceCollection services)
    {
        services.AddCustomSettingsWithValidationOnStart<EntityFrameworkSettings>("EntityFramework");

        return services;
    }
}
