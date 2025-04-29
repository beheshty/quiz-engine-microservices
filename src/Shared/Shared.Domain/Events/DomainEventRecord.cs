namespace QuizEngineMicroservices.Shared.Domain.Events;

public class DomainEventRecord : IDomainEventData
{
    public object EventData { get; }
    public long EventOrder { get; }
    public EventPublishType DispatchType { get; }

    public DomainEventRecord(object eventData, long eventOrder, EventPublishType dispatchType = EventPublishType.Local)
    {
        EventData = eventData;
        EventOrder = eventOrder;
        DispatchType = dispatchType;
    }
}