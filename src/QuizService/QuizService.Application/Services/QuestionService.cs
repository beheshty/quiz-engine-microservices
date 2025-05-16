using QuizService.Application.Grpc;
using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionGrpcClient _questionGrpcClient;

    public QuestionService(IQuestionGrpcClient questionGrpcClient)
    {
        _questionGrpcClient = questionGrpcClient;
    }

    public async Task ValidateQuestionsExistAsync(IEnumerable<Guid> questionIds, CancellationToken cancellationToken = default)
    {
        var response = await GetQuestionsByIdsAsync(questionIds, cancellationToken);
        var foundQuestionIds = response.Questions.Select(q => Guid.Parse(q.Id)).ToHashSet();
        var missingQuestionIds = questionIds.Where(id => !foundQuestionIds.Contains(id)).ToList();
        if (missingQuestionIds.Count != 0)
        {
            throw new InvalidOperationException($"The following questions do not exist: {string.Join(", ", missingQuestionIds)}");
        }
    }

    public async Task<GetQuestionsResponse> GetQuestionsByIdsAsync(IEnumerable<Guid> questionIds, CancellationToken cancellationToken = default)
    {
        var request = new GetQuestionsRequest
        {
            QuestionIds = { questionIds.Select(id => id.ToString()) }
        };
        return await _questionGrpcClient.GetQuestionsByIdsAsync(request, cancellationToken);
    }
} 