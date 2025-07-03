using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Exceptions;
using BuildingBlocks.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace QuizService.Infrastructure.Repositories.Base;

public abstract class EfRepository<TEntity> : EfReadOnlyRepository<TEntity>, IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected EfRepository(DbContext context) : base(context)
    {
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        if (autoSave)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        return entity;
    }

    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await DbSet.AddRangeAsync(entities, cancellationToken);
        if (autoSave)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        if (autoSave)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        return entity;
    }

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        DbSet.UpdateRange(entities);
        if (autoSave)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        if (autoSave)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        DbSet.RemoveRange(entities);
        if (autoSave)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}

public abstract class EfRepository<TEntity, TKey> : EfRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected EfRepository(DbContext context) : base(context)
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
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }
} 