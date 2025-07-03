using BuildingBlocks.Domain.Auditing;
using QuizService.Domain.Enums;

namespace QuizService.Domain.Entities.QuizManagement;

public class Quiz : AuditedAggregateRoot<Guid>
{
    public Quiz(Guid id, string title, string description) : this()
    {
        Id = id;
        Title = title;
        Description = description;
    }
    private Quiz() 
    {
        Status = QuizStatus.Draft;
    }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizStatus Status { get; private set; }
    public ICollection<QuizQuestion> Questions { get; private set; } = [];

    public void AddQuestions(IEnumerable<QuizQuestion> questions)
    {
        foreach (var q in questions)
        {
            Questions.Add(q);
        }
    }

    public void ChangeStatus(QuizStatus newStatus)
    {
        if (Status != QuizStatus.Draft && newStatus == QuizStatus.Draft) 
        {
            throw new InvalidOperationException("Cannot change status to Draft");
        }
        Status = newStatus;
    }
}