using QuizService.Application.Common.CQRS.Interfaces;
using System;

namespace QuizService.Application.Commands.DraftUserQuiz;

/// <summary>
/// Command to draft (start) a user quiz instance for a given user and quiz.
/// </summary>
/// <param name="UserId">The user starting the quiz.</param>
/// <param name="QuizId">The quiz to be started.</param>
public record DraftUserQuizCommand(Guid UserId, Guid QuizId) : ICommand<DraftUserQuizResultDto>; 