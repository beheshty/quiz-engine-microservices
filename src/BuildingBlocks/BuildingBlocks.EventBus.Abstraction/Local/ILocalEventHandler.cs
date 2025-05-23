namespace BuildingBlocks.EventBus.Abstraction.Local;

public interface ILocalEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : class
{
}