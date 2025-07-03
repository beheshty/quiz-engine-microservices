using BuildingBlocks.Domain.Repositories;
using QuizService.Domain.Entities.QuizRuntime;

namespace QuizService.Domain.Repositories
{
    public interface IUserQuizRepository : IRepository<UserQuiz, Guid>
    {
    }
}
