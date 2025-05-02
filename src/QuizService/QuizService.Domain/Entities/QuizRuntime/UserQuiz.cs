using QuizEngineMicroservices.Shared.Domain.Auditing;
using QuizService.Domain.Enums;

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
        StartedAt = DateTime.Now;
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
    public ICollection<UserAnswer> Answers { get; set; } = [];

    private void ChangeStatus(UserQuizStatus status)
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
}