using QuizService.Application.Common.CQRS.Interfaces;

namespace QuizService.Application.Commands.CreateQuiz;

public record CreateQuizCommand(CreateQuizDto Quiz) : ICommand<Guid>; 