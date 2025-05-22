using QuizService.Domain.Enums;

namespace QuizService.Application.Commands.ProcessUserQuiz;

public class ProcessUserQuizResultDto
{
    public Guid UserQuizId { get; set; }
    public UserQuizStatus Status { get; set; }
}
