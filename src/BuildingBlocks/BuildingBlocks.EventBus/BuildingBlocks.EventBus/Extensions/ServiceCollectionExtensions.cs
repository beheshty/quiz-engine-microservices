using BuildingBlocks.EventBus.Abstraction.Domain;
using BuildingBlocks.EventBus.Abstraction.Local;
using BuildingBlocks.EventBus.Domain;
using BuildingBlocks.EventBus.Local;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services)
    {
        services.AddScoped<ILocalEventBus, LocalEventBus>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}