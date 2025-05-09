
namespace QuizService.Application.Commands.UpdateQuiz;

public class UpdateQuizDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<QuizQuestionDto> Questions { get; set; } = [];
}

public class QuizQuestionDto
{
    public Guid QuestionId { get; set; }
    public int Order { get; set; }
}
