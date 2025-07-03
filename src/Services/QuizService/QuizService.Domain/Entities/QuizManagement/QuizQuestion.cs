using BuildingBlocks.Domain.Auditing;

namespace QuizService.Domain.Entities.QuizManagement;

public class QuizQuestion : AuditedEntity<Guid>
{
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public int Order { get; set; }
} 