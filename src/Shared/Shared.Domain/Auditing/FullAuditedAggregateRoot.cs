namespace QuizEngineMicroservices.Shared.Domain.Auditing;

public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IDeletionAuditedEntity, ISoftDelete
{
    public virtual bool IsDeleted { get; set; }

    public virtual Guid? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }
}

[Serializable]
public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IDeletionAuditedEntity, ISoftDelete
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }

    protected FullAuditedAggregateRoot()
    {

    }

    protected FullAuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}