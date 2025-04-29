using QuizEngineMicroservices.Shared.Domain;
using System.Linq.Expressions;

namespace Shared.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> where TEntity : class, IEntity
{
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity> where TEntity : class, IEntity<TKey>
{
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default);
}
