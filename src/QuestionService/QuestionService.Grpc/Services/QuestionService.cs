using Grpc.Core;

namespace QuestionService.Grpc.Services
{
    public class QuestionService : Questions.QuestionsBase
    {
        private readonly ILogger<QuestionService> _logger;
        public QuestionService(ILogger<QuestionService> logger)
        {
            _logger = logger;
        }

        public override Task<GetQuestionsResponse> GetQuestionsByIds(GetQuestionsRequest request, ServerCallContext context)
        {
            return base.GetQuestionsByIds(request, context);
        }
    }
}
