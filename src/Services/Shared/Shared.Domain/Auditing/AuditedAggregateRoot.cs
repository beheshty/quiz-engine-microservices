namespace QuizEngineMicroservices.Shared.Domain.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="IAuditedObject"/> for aggregate roots.
/// </summary>
[Serializable]
public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IModificationAuditedEntity
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }
}

[Serializable]
public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IModificationAuditedEntity
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }

    protected AuditedAggregateRoot()
    {

    }

    protected AuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}