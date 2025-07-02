namespace NotificationService.Application.DTOs;

public record UserQuizDto(
    Guid UserId,
    Guid QuizId,
    DateTime StartedAt,
    DateTime? CompletedAt,
    int? TotalScore,
    int? CorrectCount,
    int? WrongCount,
    TimeSpan? TimeTaken
);
