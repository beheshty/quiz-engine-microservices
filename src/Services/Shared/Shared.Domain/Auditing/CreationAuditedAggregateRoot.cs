namespace QuizEngineMicroservices.Shared.Domain.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedEntity"/> for aggregate roots.
/// </summary>
[Serializable]
public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedEntity
{
    public virtual DateTime CreationTime { get; set; } = DateTime.UtcNow;
    public virtual Guid? CreatorId { get; set; }
}

[Serializable]
public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedEntity
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; set; }

    protected CreationAuditedAggregateRoot()
    {
    }

    protected CreationAuditedAggregateRoot(TKey id)
        : base(id)
    {
    }
} 