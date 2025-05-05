using QuizService.Domain.Entities.QuizRuntime;
using Shared.Domain.Repositories;

namespace QuizService.Domain.Repositories
{
    public interface IUserQuizRepository : IRepository<UserQuiz, Guid>
    {
    }
}
