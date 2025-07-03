using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using QuizService.Domain.Filters;
using QuizService.Infrastructure.Data;
using QuizService.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Domain.Exceptions;

namespace QuizService.Infrastructure.Repositories;

public class QuizRepository : EfRepository<Quiz, Guid>, IQuizRepository
{
    public QuizRepository(QuizDbContext context) : base(context)
    {
    }

    public async Task<List<Quiz>> GetListAsync(QuizFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(q => q.Title.Contains(filter.Title));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(q => q.Status == filter.Status.Value);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(q => q.CreationTime >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(q => q.CreationTime <= filter.CreatedTo.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public override Task<Quiz> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = DbSet.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(Quiz), id);
        }
        return entity;
    }
} 