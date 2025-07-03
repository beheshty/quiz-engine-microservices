using BuildingBlocks.Domain.Auditing;

namespace QuizService.Domain.Entities.QuizRuntime;

public class UserAnswer : AuditedEntity<Guid>
{
    public Guid UserQuizId { get; set; }
    public Guid QuizQuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
} 