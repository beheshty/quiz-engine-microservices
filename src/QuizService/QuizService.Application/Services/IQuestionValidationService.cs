
namespace QuizService.Application.Services;

public interface IQuestionValidationService
{
    Task ValidateQuestionsExistAsync(IEnumerable<Guid> questionIds, CancellationToken cancellationToken = default);
} 