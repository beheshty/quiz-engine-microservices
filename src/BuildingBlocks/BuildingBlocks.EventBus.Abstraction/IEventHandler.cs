
namespace BuildingBlocks.EventBus.Abstraction
{
    public interface IEventHandler<TEvent> where TEvent : class
    {
        Task HandleAsync(TEvent input, CancellationToken cancellationToken = default);
        void Handle(TEvent input);
    }
}
