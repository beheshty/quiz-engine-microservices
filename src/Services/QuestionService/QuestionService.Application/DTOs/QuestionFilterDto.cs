using QuestionService.Domain.Enums;

namespace QuestionService.Application.DTOs;

public class QuestionFilterDto
{
    public string? SearchText { get; set; }
    public DifficultyLevel? DifficultyLevel { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 