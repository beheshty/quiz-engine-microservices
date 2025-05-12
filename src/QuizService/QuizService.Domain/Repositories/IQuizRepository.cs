using QuizService.Domain.Entities.QuizManagement;
using Shared.Domain.Repositories;
using QuizService.Domain.Filters;

namespace QuizService.Domain.Repositories
{
    public interface IQuizRepository : IRepository<Quiz, Guid>
    {
        Task<List<Quiz>> GetListAsync(QuizFilterDto filter, CancellationToken cancellationToken = default);
    }
}
