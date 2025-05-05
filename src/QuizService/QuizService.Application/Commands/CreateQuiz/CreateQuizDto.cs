using System.ComponentModel.DataAnnotations;

namespace QuizService.Application.Commands.CreateQuiz;

public class CreateQuizDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public List<QuizQuestionDto> Questions { get; set; } = [];
}

public class QuizQuestionDto
{
    [Required]
    public Guid QuestionId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Order { get; set; }
} 