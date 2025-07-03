using BuildingBlocks.Domain.Auditing;
using QuestionService.Domain.Enums;

namespace QuestionService.Domain.Entities;

public class Question : AuditedAggregateRoot<Guid>
{
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
        CreationTime = DateTime.UtcNow;
    }

    public void Update(string text, DifficultyLevel difficultyLevel)
    {
        Text = text;
        DifficultyLevel = difficultyLevel;
        LastModificationTime = DateTime.UtcNow;
    }

    public void AddOption(Option option)
    {
        ArgumentNullException.ThrowIfNull(option);

        Options.Add(option);
        LastModificationTime = DateTime.UtcNow;
    }

    public void AddOptions(params Option[] options)
    {
        ArgumentNullException.ThrowIfNull(options);

        foreach (var option in options)
        {
            AddOption(option);
        }
    }
}