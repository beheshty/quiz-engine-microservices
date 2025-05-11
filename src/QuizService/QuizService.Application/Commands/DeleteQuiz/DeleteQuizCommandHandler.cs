using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using QuizService.Domain.Enums;
using Shared.Domain.Exceptions;

namespace QuizService.Application.Commands.DeleteQuiz;

public class DeleteQuizCommandHandler : ICommandHandler<DeleteQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;

    public DeleteQuizCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<Guid> Handle(DeleteQuizCommand command, CancellationToken cancellationToken = default)
    {
        var quiz = await _quizRepository.GetAsync(command.Id, cancellationToken);
        if (quiz == null)
        {
            throw new EntityNotFoundException(typeof(Quiz), command.Id);
        }

        if (quiz.Status != QuizStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot delete quiz with status {quiz.Status}. Only quizzes in Draft status can be deleted.");
        }

        await _quizRepository.DeleteAsync(quiz, true, cancellationToken);

        return quiz.Id;
    }
} 