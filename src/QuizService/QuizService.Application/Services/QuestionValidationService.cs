using QuizService.Application.Grpc;
using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Services;

public class QuestionValidationService : IQuestionValidationService
{
    private readonly IQuestionGrpcClient _questionGrpcClient;

    public QuestionValidationService(IQuestionGrpcClient questionGrpcClient)
    {
        _questionGrpcClient = questionGrpcClient;
    }

    public async Task ValidateQuestionsExistAsync(IEnumerable<Guid> questionIds, CancellationToken cancellationToken = default)
    {
        var request = new GetQuestionsRequest
        {
            QuestionIds = { questionIds.Select(id => id.ToString()) }
        };

        var response = await _questionGrpcClient.GetQuestionsByIdsAsync(request, cancellationToken);
        
        var foundQuestionIds = response.Questions.Select(q => Guid.Parse(q.Id)).ToHashSet();
        var missingQuestionIds = questionIds.Where(id => !foundQuestionIds.Contains(id)).ToList();

        if (missingQuestionIds.Count != 0)
        {
            throw new InvalidOperationException($"The following questions do not exist: {string.Join(", ", missingQuestionIds)}");
        }
    }
} 