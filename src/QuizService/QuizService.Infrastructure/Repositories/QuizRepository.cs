using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using QuizService.Infrastructure.Data;
using QuizService.Infrastructure.Repositories.Base;

namespace QuizService.Infrastructure.Repositories;

public class QuizRepository : EfRepository<Quiz, Guid>, IQuizRepository
{
    public QuizRepository(QuizDbContext context) : base(context)
    {
    }
} 