namespace QuizEngineMicroservices.Shared.Domain.Auditing;
public abstract class CreationAuditedEntity : Entity, ICreationAuditedEntity
{
    public virtual DateTime CreationTime { get; set; } = DateTime.UtcNow;

    public virtual Guid? CreatorId { get; set; }
}

[Serializable]
public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedEntity
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; set; }

    protected CreationAuditedEntity()
    {

    }

    protected CreationAuditedEntity(TKey id)
        : base(id)
    {

    }
}