using BuildingBlocks.EventBus.Abstraction;
using BuildingBlocks.EventBus.Abstraction.Distributed;
using BuildingBlocks.EventBus.Abstraction.Domain;
using BuildingBlocks.EventBus.Abstraction.Local;

namespace BuildingBlocks.EventBus.Domain;

public class DomainEventDispatcher(ILocalEventBus localEventBus, IDistributedEventBus distributedEventBus)
    : IDomainEventDispatcher
{
    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEventData
    {
        switch (domainEvent.PublishType)
        {
            case PublishType.Distributed:
                await distributedEventBus.PublishAsync(domainEvent.EventData, cancellationToken);
                break;
            case PublishType.Local:
                await localEventBus.PublishAsync(domainEvent.EventData, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(domainEvent.PublishType));
        }
    }
}