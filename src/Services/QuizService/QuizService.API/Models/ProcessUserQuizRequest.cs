using QuizService.Application.Commands.ProcessUserQuiz;

namespace QuizService.API.Models
{
    public class ProcessUserQuizRequest
    {
        public Guid UserQuizId { get; set; }
        public List<UserAnswerRequest> Answers { get; set; }
    }
} 