namespace QuizService.API.Models
{
    public class UserAnswerRequest
    {
        public Guid QuizQuestionId { get; set; }
        public string AnswerText { get; set; }
    }
} 