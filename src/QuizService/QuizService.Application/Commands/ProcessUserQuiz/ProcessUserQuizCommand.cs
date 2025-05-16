using QuizService.Application.Common.CQRS.Interfaces;
using System;
using System.Collections.Generic;

namespace QuizService.Application.Commands.ProcessUserQuiz;

public record ProcessUserQuizCommand(Guid UserQuizId, Guid UserId, List<UserQuizAnswerDto> Answers) : ICommand<ProcessUserQuizResultDto>;

public class UserQuizAnswerDto
{
    public Guid QuizQuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
} 