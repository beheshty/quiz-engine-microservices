namespace QuizEngineMicroservices.Shared.Domain.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="IAuditedObject"/> for aggregate roots.
/// </summary>
[Serializable]
public abstract class AuditedEntity : CreationAuditedEntity, IModificationAuditedEntity
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }
}

[Serializable]
public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IModificationAuditedEntity
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }

    protected AuditedEntity()
    {

    }

    protected AuditedEntity(TKey id)
        : base(id)
    {

    }
}