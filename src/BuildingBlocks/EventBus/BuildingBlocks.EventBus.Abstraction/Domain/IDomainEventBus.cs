namespace BuildingBlocks.EventBus.Abstraction.Domain;

public interface IDomainEventBus
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEventData;
}