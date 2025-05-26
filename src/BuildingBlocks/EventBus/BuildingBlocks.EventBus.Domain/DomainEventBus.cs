using BuildingBlocks.EventBus.Abstraction;
using BuildingBlocks.EventBus.Abstraction.Distributed;
using BuildingBlocks.EventBus.Abstraction.Domain;
using BuildingBlocks.EventBus.Abstraction.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.EventBus.Domain;

public class DomainEventBus : IDomainEventBus
{
    private readonly ILocalEventBus _localEventBus;
    private readonly IDistributedEventBus? _distributedEventBus;
    private readonly DomainEventBusOptions _options;


    public DomainEventBus(
        ILocalEventBus localEventBus,
        IServiceProvider serviceProvider,
        IOptions<DomainEventBusOptions> options)
    {
        _localEventBus = localEventBus;
        _options = options.Value;

        if (_options.UseDistributed)
        {
            _distributedEventBus = serviceProvider.GetService<IDistributedEventBus>();

            if (_distributedEventBus == null)
                throw new InvalidOperationException("DistributedEventBus is required but not registered.");
        }
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : IDomainEventData
    {
        switch (domainEvent.PublishType)
        {
            case PublishType.Distributed:
                if (_options.UseDistributed)
                {
                    await _distributedEventBus!.PublishAsync(domainEvent.EventData, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException("DistributedEventBus is required but not registered.");
                }
                break;
            case PublishType.Local:
                if (_options.UseLocal)
                {
                    await _localEventBus.PublishAsync(domainEvent.EventData, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException("LocalEvent is required but not registered.");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(domainEvent.PublishType));
        }
    }
}