using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Grpc;

public interface IQuestionGrpcClient
{
    Task<GetQuestionsResponse> GetQuestionsByIdsAsync(GetQuestionsRequest request, CancellationToken cancellationToken = default);
} 