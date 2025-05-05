using QuizService.Domain.Entities.QuizManagement;
using Shared.Domain.Repositories;

namespace QuizService.Domain.Repositories
{
    public interface IQuizRepository : IRepository<Quiz, Guid>
    {
    }
}
