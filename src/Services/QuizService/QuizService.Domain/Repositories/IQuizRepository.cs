using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Filters;
using BuildingBlocks.Domain.Repositories;

namespace QuizService.Domain.Repositories
{
    public interface IQuizRepository : IRepository<Quiz, Guid>
    {
        Task<List<Quiz>> GetListAsync(QuizFilterDto filter, CancellationToken cancellationToken = default);
    }
}
