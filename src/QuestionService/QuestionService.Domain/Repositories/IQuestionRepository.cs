
using QuestionService.Domain.Entities;
using Shared.Domain.Repositories;

namespace QuestionService.Domain.Repositories;

public interface IQuestionRepository : IRepository<Question, Guid>
{
}
