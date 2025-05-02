using QuizEngineMicroservices.Shared.Domain.Auditing;

namespace QuizService.Domain.Entities.QuizManagement;

public class QuizQuestion : AuditedEntity<Guid>
{
    public Guid QuestionId { get; set; }
    public int Order { get; set; }
} 