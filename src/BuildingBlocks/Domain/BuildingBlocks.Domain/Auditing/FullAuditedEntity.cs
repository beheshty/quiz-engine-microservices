namespace BuildingBlocks.Domain.Auditing;
/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited aggregate roots.
/// </summary>
[Serializable]
public abstract class FullAuditedEntity : AuditedEntity, IDeletionAuditedEntity
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }
}

[Serializable]
public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IDeletionAuditedEntity
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }

    protected FullAuditedEntity()
    {

    }

    protected FullAuditedEntity(TKey id)
        : base(id)
    {

    }
}