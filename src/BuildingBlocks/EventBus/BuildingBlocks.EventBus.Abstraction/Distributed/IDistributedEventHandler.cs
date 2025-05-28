namespace BuildingBlocks.EventBus.Abstraction.Distributed;

/// <summary>
/// Handle the distributed events
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IDistributedEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : class
{
}