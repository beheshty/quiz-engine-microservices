namespace BuildingBlocks.Domain;

public interface IEntity
{
    /// <summary>
    /// Returns an array of ordered keys for this entity.
    /// </summary>
    /// <returns></returns>
    object?[] GetKeys();
}

public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>
    TKey Id { get; }
}