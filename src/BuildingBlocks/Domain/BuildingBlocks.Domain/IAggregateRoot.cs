namespace BuildingBlocks.Domain;

/// <summary>
/// Defines an aggregate root. It's primary key may not be "Id" or it may have a composite primary key.
/// Use <see cref="IAggregateRoot{TKey}"/> where possible for better integration to repositories and other structures in the framework.
/// </summary>
public interface IAggregateRoot : IEntity
{

}

public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
{

}