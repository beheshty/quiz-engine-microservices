using BuildingBlocks.EventBus.Abstraction.Local;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.Local.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalEventBus(this IServiceCollection services)
    {
        services.AddScoped<ILocalEventBus, LocalEventBus>();

        return services;
    }
}