using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Services;

public interface IQuestionService
{
    Task ValidateQuestionsExistAsync(IEnumerable<Guid> questionIds, CancellationToken cancellationToken = default);
    Task<GetQuestionsResponse> GetQuestionsByIdsAsync(IEnumerable<Guid> questionIds, CancellationToken cancellationToken = default);
} 