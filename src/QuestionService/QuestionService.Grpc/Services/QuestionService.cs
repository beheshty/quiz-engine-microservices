using Grpc.Core;
using QuestionService.Application.Services;
using Shared.Question.Grpc.Proto;

namespace QuestionService.Grpc.Services
{
    public class QuestionService(IQuestionAppService questionAppService, ILogger<QuestionService> logger) : Questions.QuestionsBase
    {
        public override async Task<GetQuestionsResponse> GetQuestionsByIds(GetQuestionsRequest request, ServerCallContext context)
        {
            var questionIds = request.QuestionIds.Select(Guid.Parse).ToArray();
            var questions = await questionAppService.GetQuestionByIdsAsync(questionIds);
            return new GetQuestionsResponse { Questions = { questions.Select(q => new Question { Id = q.Id.ToString(), Text = q.Text, DifficultyLevel = (int)q.DifficultyLevel }) } };
        }
    }
}
