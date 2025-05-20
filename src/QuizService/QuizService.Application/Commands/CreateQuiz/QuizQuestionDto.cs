using System.ComponentModel.DataAnnotations;

namespace QuizService.Application.Commands.CreateQuiz;

public class QuizQuestionDto
{
    [Required]
    public Guid QuestionId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Order { get; set; }
} 