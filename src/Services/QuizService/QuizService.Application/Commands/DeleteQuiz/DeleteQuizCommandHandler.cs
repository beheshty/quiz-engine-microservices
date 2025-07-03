using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using QuizService.Domain.Enums;
using BuildingBlocks.Domain.Exceptions;

namespace QuizService.Application.Commands.DeleteQuiz;

public class DeleteQuizCommandHandler : ICommandHandler<DeleteQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuizCommandHandler(IQuizRepository quizRepository, IUnitOfWork unitOfWork)
    {
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
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

        await _quizRepository.DeleteAsync(quiz, cancellationToken: cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return quiz.Id;
    }
} 