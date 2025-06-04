namespace BuildingBlocks.EventBus.Abstraction.Domain
{
    public interface IDomainEventData
    {
        object EventData { get; }
        long EventOrder { get; }
        PublishType PublishType { get; }
    }
}
