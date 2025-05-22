using QuestionService.Domain.Enums;

namespace QuestionService.Application.DTOs;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; }
    public List<OptionDto> Options { get; set; } = [];
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}

public class OptionDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int Order { get; set; }
} 