using BuildingBlocks.Domain.Exceptions;
using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;

namespace QuizService.Application.Commands.DraftUserQuiz;

public class DraftUserQuizCommandHandler : ICommandHandler<DraftUserQuizCommand, DraftUserQuizResultDto>
{
    private readonly IUserQuizRepository _userQuizRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DraftUserQuizCommandHandler(
        IUserQuizRepository userQuizRepository,
        IQuizRepository quizRepository,
        IUnitOfWork unitOfWork)
    {
        _userQuizRepository = userQuizRepository;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DraftUserQuizResultDto> Handle(DraftUserQuizCommand command, CancellationToken cancellationToken = default)
    {
        var quiz = await _quizRepository.GetAsync(command.QuizId, cancellationToken);
        if (quiz == null)
            throw new EntityNotFoundException(typeof(Quiz), command.QuizId);
        //TODO: check if user exist
        if (quiz.Status != QuizStatus.Published)
            throw new InvalidOperationException("Quiz must be published to start a user quiz.");

        var userQuiz = new UserQuiz(Guid.NewGuid(), command.UserId, command.QuizId);
        await _userQuizRepository.InsertAsync(userQuiz, cancellationToken: cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        var result = new DraftUserQuizResultDto
        {
            UserQuizId = userQuiz.Id,
            QuizId = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            Status = quiz.Status,
            Questions = [.. quiz.Questions
                .OrderBy(q => q.Order)
                .Select(q => new DraftUserQuizQuestionDto
                {
                    QuizQuestionId = q.Id,
                    QuestionId = q.QuestionId,
                    Order = q.Order
                })]
        };

        return result;
    }
} 