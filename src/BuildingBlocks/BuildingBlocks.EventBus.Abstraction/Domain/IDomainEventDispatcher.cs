namespace BuildingBlocks.EventBus.Abstraction.Domain;

public interface IDomainEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEventData;
}