using BuildingBlocks.EventBus.Abstraction.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainEventBus(this IServiceCollection services, Action<DomainEventBusOptions> setup)
    {
        services.Configure(setup);
        services.AddScoped<IDomainEventBus, DomainEventBus>();
        return services;
    }
}