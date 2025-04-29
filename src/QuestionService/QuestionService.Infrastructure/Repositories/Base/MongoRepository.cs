using MongoDB.Driver;
using QuizEngineMicroservices.Shared.Domain;
using QuestionService.Infrastructure.Exceptions;
using Shared.Domain.Repositories;

namespace QuestionService.Infrastructure.Repositories.Base;

public abstract class MongoRepository<TEntity> : MongoReadOnlyRepository<TEntity>, IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected MongoRepository(IMongoCollection<TEntity> collection) : base(collection)
    {
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity;
    }

    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await Collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.GetKeys(), entity.GetKeys());
        await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        return entity;
    }

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var bulkOps = entities.Select(entity =>
            new ReplaceOneModel<TEntity>(
                Builders<TEntity>.Filter.Eq(e => e.GetKeys(), entity.GetKeys()),
                entity)
            { IsUpsert = true });

        await Collection.BulkWriteAsync(bulkOps, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.GetKeys(), entity.GetKeys());
        await Collection.DeleteOneAsync(filter, cancellationToken);
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var bulkOps = entities.Select(entity =>
            new DeleteOneModel<TEntity>(
                Builders<TEntity>.Filter.Eq(e => e.GetKeys(), entity.GetKeys())));

        await Collection.BulkWriteAsync(bulkOps, cancellationToken: cancellationToken);
    }
}

public abstract class MongoRepository<TEntity, TKey> : MongoRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected MongoRepository(IMongoCollection<TEntity> collection) : base(collection)
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

    public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        await Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        return entity;
    }

    public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        await Collection.DeleteOneAsync(filter, cancellationToken);
    }
}