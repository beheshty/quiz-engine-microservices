using QuizService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace QuizService.Application.Commands.DraftUserQuiz;

public class DraftUserQuizResultDto
{
    public Guid UserQuizId { get; set; }
    public Guid QuizId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizStatus Status { get; set; }
    public List<DraftUserQuizQuestionDto> Questions { get; set; } = new();
}

public class DraftUserQuizQuestionDto
{
    public Guid QuizQuestionId { get; set; }
    public Guid QuestionId { get; set; }
    public int Order { get; set; }
} 