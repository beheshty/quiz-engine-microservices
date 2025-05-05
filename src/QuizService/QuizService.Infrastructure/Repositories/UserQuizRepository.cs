using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Repositories;
using QuizService.Infrastructure.Data;
using QuizService.Infrastructure.Repositories.Base;

namespace QuizService.Infrastructure.Repositories;

public class UserQuizRepository : EfRepository<UserQuiz, Guid>, IUserQuizRepository
{
    public UserQuizRepository(QuizDbContext context) : base(context)
    {
    }
} 