using BuildingBlocks.Domain.Auditing;
using QuizService.Domain.Enums;
using QuizService.IntegrationEvents.UserQuiz;

namespace QuizService.Domain.Entities.QuizRuntime;

public class UserQuiz : AuditedAggregateRoot<Guid>
{
    public UserQuiz(Guid id, Guid userId, Guid quizId) : this()
    {
        Id = id;
        UserId = userId;
        QuizId = quizId;
    }

    private UserQuiz()
    {
        StartedAt = DateTime.UtcNow;
        Status = UserQuizStatus.NotStarted;
    }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public UserQuizStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; set; }
    public int? TotalScore { get; set; }
    public int? CorrectCount { get; set; }
    public int? WrongCount { get; set; }
    public TimeSpan? TimeTaken { get; set; }
    public virtual ICollection<UserAnswer> Answers { get; set; } = [];

    public void ChangeStatus(UserQuizStatus status)
    {
        Status = status;
    }

    public void AddAnswer(UserAnswer answer)
    {
        if (Status == UserQuizStatus.NotStarted)
        {
            ChangeStatus(UserQuizStatus.InProgress);
        }
        Answers.Add(answer);
    }

    public void ProcessAnswers(IEnumerable<UserAnswer> newAnswers, IDictionary<Guid, Guid> correctAnswers)
    {
        if (Status == UserQuizStatus.Completed)
            throw new InvalidOperationException("Quiz already completed.");

        if(newAnswers.Any(a => !correctAnswers.Any(c => c.Key == a.QuizQuestionId)))
        {
            throw new InvalidOperationException("Questions does not exist in the quiz.");
        }

        foreach (var answer in newAnswers)
        {
            // Check if already answered
            if (Answers.Any(a => a.QuizQuestionId == answer.QuizQuestionId))
                continue;
            // Set correctness
            if (correctAnswers.TryGetValue(answer.QuizQuestionId, out Guid correct))
            {
                answer.IsCorrect = Guid.Parse(answer.AnswerText) == correct;
            }
            AddAnswer(answer);
        }
        if (Answers.Count >= correctAnswers.Count)
        {
            CompleteQuiz();
        }
    }

    private void CompleteQuiz()
    {
        ChangeStatus(UserQuizStatus.Completed);
        
        CompletedAt = DateTime.UtcNow;
        CorrectCount = Answers.Count(a => a.IsCorrect);
        WrongCount = Answers.Count(a => !a.IsCorrect);
        TimeTaken = CompletedAt - StartedAt;

        AddDistributedEvent(new UserQuizCompletedIntegrationEvent
        {
            CompletedAt = CompletedAt,
            CorrectCount = CorrectCount,
            WrongCount = WrongCount,
            TimeTaken = TimeTaken,
            QuizId = QuizId,
            UserId = UserId
        });
    }
}