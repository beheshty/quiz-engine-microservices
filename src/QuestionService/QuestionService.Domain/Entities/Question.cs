using QuestionService.Domain.Enums;
using QuizEngineMicroservices.Shared.Domain.Auditing;

namespace QuestionService.Domain.Entities;

public class Question : AuditedAggregateRoot<Guid>
{
    public Guid QuizId { get; private set; }
    public string Text { get; private set; }
    public DifficultyLevel DifficultyLevel { get; private set; }
    public ICollection<Option> Options { get; private set; }

    private Question() { }

    public Question(
        string text,
        DifficultyLevel difficultyLevel)
    {
        Id = Guid.NewGuid();
        Text = text;
        DifficultyLevel = difficultyLevel;
        Options = [];
    }

    public void Update(string text, DifficultyLevel difficultyLevel)
    {
        Text = text;
        DifficultyLevel = difficultyLevel;
    }

    public void AddOption(Option option)
    {
        ArgumentNullException.ThrowIfNull(option);

        Options.Add(option);
    }
} 