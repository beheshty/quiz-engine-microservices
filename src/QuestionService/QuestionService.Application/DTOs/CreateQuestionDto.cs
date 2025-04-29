using System.ComponentModel.DataAnnotations;
using QuestionService.Domain.Enums;

namespace QuestionService.Application.DTOs;

public class CreateQuestionDto
{
    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public DifficultyLevel DifficultyLevel { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(6)]
    public List<CreateOptionDto> Options { get; set; } = [];
}

public class CreateOptionDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public bool IsCorrect { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Order { get; set; }
} 