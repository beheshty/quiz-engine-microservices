namespace BuildingBlocks.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public Type EntityType { get; }
    public object Id { get; }

    public EntityNotFoundException(Type entityType, object id)
        : base($"Entity of type {entityType.Name} with id {id} was not found")
    {
        EntityType = entityType;
        Id = id;
    }
} 