using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Exceptions;
using BuildingBlocks.Domain.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace QuestionService.Infrastructure.Repositories.Base;

public abstract class MongoReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly IMongoCollection<TEntity> Collection;

    protected MongoReadOnlyRepository(IMongoCollection<TEntity> collection)
    {
        Collection = collection;
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(FilterDefinition<TEntity>.Empty).AnyAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(predicate).AnyAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(FilterDefinition<TEntity>.Empty).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.CountDocumentsAsync(FilterDefinition<TEntity>.Empty, cancellationToken: cancellationToken);
    }
}

public abstract class MongoReadOnlyRepository<TEntity, TKey> : MongoReadOnlyRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected MongoReadOnlyRepository(IMongoCollection<TEntity> collection) : base(collection)
    {
    }

    public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }
        return entity;
    }

    public virtual async Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}