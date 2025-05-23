namespace BuildingBlocks.EventBus.Abstraction.Distributed;

/// <summary>
/// Handle the distributed events
/// No UOW will be started automatically
/// No User will be selected automatically
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IDistributedEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : class
{
}