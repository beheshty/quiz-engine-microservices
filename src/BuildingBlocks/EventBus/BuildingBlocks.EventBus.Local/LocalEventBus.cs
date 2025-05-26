
using BuildingBlocks.EventBus.Abstraction.Local;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.Local;

public class LocalEventBus(IServiceProvider serviceProvider) : ILocalEventBus
{
    public async Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default) where TEvent : class
    {
        var handlers = serviceProvider.GetServices<ILocalEventHandler<TEvent>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(eventData, cancellationToken);
        }
    }
}