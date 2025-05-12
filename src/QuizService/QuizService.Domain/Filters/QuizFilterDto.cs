using QuizService.Domain.Enums;

namespace QuizService.Domain.Filters;

public class QuizFilterDto
{
    public string? Title { get; set; }
    public QuizStatus? Status { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
} 