using QuestionService.Domain.Entities;
using QuestionService.Domain.Enums;
using Shared.Domain.Repositories;

namespace QuestionService.Domain.Repositories;

public interface IQuestionRepository : IRepository<Question, Guid>
{
   Task<(IEnumerable<Question> Items, int TotalCount)> GetFilteredListAsync(string? searchText = null, DifficultyLevel? difficultyLevel = null,
                                                                         int pageNumber = 0, int pageSize = 10, CancellationToken cancellationToken = default);
}
