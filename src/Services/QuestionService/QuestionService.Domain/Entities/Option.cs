
using QuizEngineMicroservices.Shared.Domain.Auditing;

namespace QuestionService.Domain.Entities;

public class Option : AuditedEntity<Guid>
{
    private Option() { }

    public Option(string content, bool isCorrect, int order)
    {
        Id = Guid.NewGuid();
        Content = content;
        IsCorrect = isCorrect;
        Order = order;
    }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public int Order { get; set; }
} 