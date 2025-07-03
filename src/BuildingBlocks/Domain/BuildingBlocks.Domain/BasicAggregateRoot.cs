using BuildingBlocks.Domain.Events;
using BuildingBlocks.EventBus.Abstraction;

namespace BuildingBlocks.Domain;

public abstract class BasicAggregateRoot : Entity, IAggregateRoot, IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = [];
    private readonly ICollection<DomainEventRecord> _localEvents = [];

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    protected virtual void AddLocalEvent(object eventData) 
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext(), PublishType.Distributed));
    }

    public void ClearAllEvents()
    {
        ClearDistributedEvents();
        ClearLocalEvents();
    }

    public IEnumerable<DomainEventRecord> GetAllEvents()
    {
        return _distributedEvents.Concat(_localEvents);
    }

    public bool HasAnyEvents()
    {
        return _localEvents.Count > 0 || _distributedEvents.Count > 0;
    }
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = [];
    private readonly ICollection<DomainEventRecord> _localEvents = [];

    protected BasicAggregateRoot()
    {
    }

    protected BasicAggregateRoot(TKey id)
        : base(id)
    {
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext(), PublishType.Distributed));
    }

    public void ClearAllEvents()
    {
        ClearDistributedEvents();
        ClearLocalEvents();
    }

    public IEnumerable<DomainEventRecord> GetAllEvents()
    {
        return _distributedEvents.Concat(_localEvents);
    }

    public bool HasAnyEvents()
    {
        return _localEvents.Count > 0 || _distributedEvents.Count > 0;
    }
}