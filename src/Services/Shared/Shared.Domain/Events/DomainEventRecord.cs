using BuildingBlocks.EventBus.Abstraction;
using BuildingBlocks.EventBus.Abstraction.Domain;

namespace QuizEngineMicroservices.Shared.Domain.Events;

public class DomainEventRecord : IDomainEventData
{
    public object EventData { get; }
    public long EventOrder { get; }
    public PublishType PublishType { get; }

    public DomainEventRecord(object eventData, long eventOrder, PublishType publishType = PublishType.Local)
    {
        EventData = eventData;
        EventOrder = eventOrder;
        PublishType = publishType;
    }
}