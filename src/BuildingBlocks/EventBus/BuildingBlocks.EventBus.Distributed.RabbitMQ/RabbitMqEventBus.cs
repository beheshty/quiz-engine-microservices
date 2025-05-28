using BuildingBlocks.EventBus.Abstraction.Distributed;
using MassTransit;

namespace BuildingBlocks.EventBus.Distributed.RabbitMQ
{
    internal class RabbitMqEventBus : IDistributedEventBus
    {
        private readonly IBus _bus;

        internal RabbitMqEventBus(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default) where TEvent : class
        {
            await _bus.Publish(eventData, cancellationToken);
        }
    }
}
