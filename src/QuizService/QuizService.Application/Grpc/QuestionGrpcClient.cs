using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Grpc;

public class QuestionGrpcClient : IQuestionGrpcClient
{
    private readonly Questions.QuestionsClient _questionsClient;

    public QuestionGrpcClient(Questions.QuestionsClient questionsClient)
    {
        _questionsClient = questionsClient;
    }

    public async Task<GetQuestionsResponse> GetQuestionsByIdsAsync(GetQuestionsRequest request, CancellationToken cancellationToken = default)
    {
        return await _questionsClient.GetQuestionsByIdsAsync(request, cancellationToken: cancellationToken);
    }
} 