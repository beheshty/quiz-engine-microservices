using BuildingBlocks.Domain.Events;
using BuildingBlocks.EventBus.Abstraction.Domain;
using QuizService.Domain.Repositories;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuizDbContext _dbContext;
        private readonly IDomainEventBus _domainEventBus;

        public UnitOfWork(QuizDbContext dbContext, IDomainEventBus domainEventBus)
        {
            _dbContext = dbContext;
            _domainEventBus = domainEventBus;
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            await PublishDomainEvents(cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            var aggregateRoots = _dbContext.ChangeTracker.Entries<IGeneratesDomainEvents>()
                .Where(e => e.Entity.HasAnyEvents())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = CollectDomainEvents(aggregateRoots);

            if (domainEvents.Count != 0)
            {
                await PublishEvents(domainEvents, cancellationToken);
            }
        }

        private static List<DomainEventRecord> CollectDomainEvents(List<IGeneratesDomainEvents> aggregateRoots)
        {
            var domainEvents = new List<DomainEventRecord>();
            foreach (var aggregateRoot in aggregateRoots)
            {
                domainEvents.AddRange(aggregateRoot.GetAllEvents());
                aggregateRoot.ClearAllEvents();
            }

            return domainEvents;
        }

        private async Task PublishEvents(List<DomainEventRecord> domainEvents, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in domainEvents.OrderBy(e => e.EventOrder))
            {
                await _domainEventBus.PublishAsync(domainEvent, cancellationToken);
            }
        }
    }
}