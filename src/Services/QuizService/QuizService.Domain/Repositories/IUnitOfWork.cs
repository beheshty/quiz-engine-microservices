
namespace QuizService.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
} 