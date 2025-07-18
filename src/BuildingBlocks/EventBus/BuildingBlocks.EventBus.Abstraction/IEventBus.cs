﻿
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.EventBus.Abstraction
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default) where TEvent : class;
    }
}
