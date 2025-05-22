using QuizService.Domain.Enums;

namespace QuizService.Application.Queries.GetQuizzes;

public class QuizListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
} 