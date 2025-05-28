using BuildingBlocks.EventBus.Abstraction.Distributed;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.Distributed.RabbitMQ
{
    internal class RabbitMqEventConsumer<TEvent> : IConsumer<TEvent> where TEvent : class
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqEventConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<TEvent> context)
        {
            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IDistributedEventHandler<TEvent>>>();
            var bus = scope.ServiceProvider.GetRequiredService<IBus>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(context.Message, context.CancellationToken);
            }
        }
    }
}
