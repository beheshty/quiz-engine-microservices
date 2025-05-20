using Microsoft.EntityFrameworkCore;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Repositories;
using QuizService.Infrastructure.Data;
using QuizService.Infrastructure.Repositories.Base;
using Shared.Domain.Exceptions;

namespace QuizService.Infrastructure.Repositories;

public class UserQuizRepository : EfRepository<UserQuiz, Guid>, IUserQuizRepository
{
    public UserQuizRepository(QuizDbContext context) : base(context)
    {
    }

    public async override Task<UserQuiz> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.Include(u => u.Answers).FirstOrDefaultAsync(u => u.Id == id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(UserQuiz), id);
        }
        return entity;
    }
} 