using QuizService.Application.Common.CQRS.Interfaces;

namespace QuizService.Application.Commands.DeleteQuiz;

public record DeleteQuizCommand(Guid Id) : ICommand<Guid>; 