using Microsoft.Extensions.DependencyInjection;
using Service.Base.Api.CQRS;

namespace Service.Base.Api.Setup;

public static class CqrsSetupExtensions
{
    public static IServiceCollection AddCommandHandler<T, TCommandHandler>(this IServiceCollection services, Func<IServiceProvider, TCommandHandler>? configure = null)
        where TCommandHandler : class, ICommandHandler<T>
    {
        if (configure == null)
        {
            services.AddTransient<TCommandHandler, TCommandHandler>();
            services.AddTransient<ICommandHandler<T>, TCommandHandler>();
        }
        else
        {
            services.AddTransient<TCommandHandler, TCommandHandler>(configure);
            services.AddTransient<ICommandHandler<T>, TCommandHandler>(configure);
        }

        services
            .AddTransient<Func<T, CancellationToken, ValueTask>>(
                sp => sp.GetRequiredService<ICommandHandler<T>>().HandleAsync
            )
            .AddTransient<CommandHandler<T>>(
                sp => sp.GetRequiredService<ICommandHandler<T>>().HandleAsync
            );

        return services;
    }

    public static IServiceCollection AddResourceCommandHandler<T, TCommandHandler>(this IServiceCollection services, Func<IServiceProvider, TCommandHandler>? configure = null)
        where TCommandHandler : class, IResourceCommandHandler<T>
    {
        if (configure == null)
        {
            services.AddTransient<TCommandHandler, TCommandHandler>();
            services.AddTransient<IResourceCommandHandler<T>, TCommandHandler>();
        }
        else
        {
            services.AddTransient<TCommandHandler, TCommandHandler>(configure);
            services.AddTransient<IResourceCommandHandler<T>, TCommandHandler>(configure);
        }

        services
            .AddTransient<Func<T, CancellationToken, ValueTask<Guid>>>(
                sp => sp.GetRequiredService<IResourceCommandHandler<T>>().HandleAsync
            )
            .AddTransient<ResourceCommandHandler<T>>(
                sp => sp.GetRequiredService<IResourceCommandHandler<T>>().HandleAsync
            );

        return services;
    }

    public static IServiceCollection AddQueryHandler<T, TResult, TQueryHandler>(this IServiceCollection services, Func<IServiceProvider, TQueryHandler>? configure = null)
        where TQueryHandler : class, IQueryHandler<T, TResult>
    {
        if (configure == null)
        {
            services
                .AddTransient<TQueryHandler, TQueryHandler>()
                .AddTransient<IQueryHandler<T, TResult>, TQueryHandler>();
        }
        else
        {
            services
                .AddTransient<TQueryHandler, TQueryHandler>(configure)
                .AddTransient<IQueryHandler<T, TResult>, TQueryHandler>(configure);
        }

        services
            .AddTransient<Func<T, CancellationToken, ValueTask<TResult>>>(
                sp => sp.GetRequiredService<IQueryHandler<T, TResult>>().HandleAsync
            )
            .AddTransient<QueryHandler<T, TResult>>(
                sp => sp.GetRequiredService<IQueryHandler<T, TResult>>().HandleAsync
            );

        return services;
    }
}
