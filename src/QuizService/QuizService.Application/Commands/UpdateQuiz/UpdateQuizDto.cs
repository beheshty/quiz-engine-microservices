using QuizService.Application.Commands.CreateQuiz;
using System.ComponentModel.DataAnnotations;

namespace QuizService.Application.Commands.UpdateQuiz;

public class UpdateQuizDto
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

