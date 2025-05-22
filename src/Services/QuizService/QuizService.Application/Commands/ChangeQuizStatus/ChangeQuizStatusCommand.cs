using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Enums;

namespace QuizService.Application.Commands.ChangeQuizStatus;

public record ChangeQuizStatusCommand : ICommand<QuizStatus>
{
    public Guid QuizId { get; init; }
    public QuizStatus NewStatus { get; init; }
} 