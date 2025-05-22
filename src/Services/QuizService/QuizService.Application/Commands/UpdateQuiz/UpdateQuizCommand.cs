using QuizService.Application.Common.CQRS.Interfaces;

namespace QuizService.Application.Commands.UpdateQuiz;

public record UpdateQuizCommand(Guid Id, UpdateQuizDto Quiz) : ICommand<Guid>;
