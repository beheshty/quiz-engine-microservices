using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Shared.Domain.Exceptions;

namespace QuizService.Application.Commands.ProcessUserQuiz;

public class ProcessUserQuizCommandHandler : ICommandHandler<ProcessUserQuizCommand, ProcessUserQuizResultDto>
{
    private readonly IUserQuizRepository _userQuizRepository;
    private readonly IQuestionService _questionService;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessUserQuizCommandHandler(
        IUserQuizRepository userQuizRepository,
        IQuestionService questionService,
        IQuizRepository quizRepository,
        IUnitOfWork unitOfWork)
    {
        _userQuizRepository = userQuizRepository;
        _questionService = questionService;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProcessUserQuizResultDto> Handle(ProcessUserQuizCommand command, CancellationToken cancellationToken = default)
    {
        var userQuiz = await _userQuizRepository.GetAsync(command.UserQuizId, cancellationToken);
        ValidateUserQuiz(userQuiz, command.UserId);

        var quizQuestionIds = command.Answers.Select(a => a.QuizQuestionId);
        var quiz = await _quizRepository.GetAsync(userQuiz.QuizId, cancellationToken);
        var questionIds = quiz.Questions.Select(a => a.QuestionId);
        var questionResponse = await _questionService.GetQuestionsByIdsAsync(questionIds, cancellationToken);
        var correctAnswers = questionResponse.Questions.ToDictionary(
            q => quiz.Questions.First(qq => qq.QuestionId.ToString() == q.Id).Id,
            q => Guid.Parse(q.CorrectAnswerOptionId)
        );

        var userAnswers = CreateUserAnswers(command);

        userQuiz.ProcessAnswers(userAnswers, correctAnswers);
       
        await _userQuizRepository.UpdateAsync(userQuiz, cancellationToken: cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ProcessUserQuizResultDto
        {
            UserQuizId = userQuiz.Id,
            Status = userQuiz.Status,
        };
    }

    private static void ValidateUserQuiz(UserQuiz? userQuiz, Guid userId)
    {
        if (userQuiz == null || userQuiz.UserId != userId)
            throw new EntityNotFoundException(typeof(UserQuiz), userQuiz?.Id ?? Guid.Empty);
        if (userQuiz.Status == UserQuizStatus.Completed)
            throw new InvalidOperationException("Quiz already completed.");
    }

    private static IEnumerable<UserAnswer> CreateUserAnswers(ProcessUserQuizCommand command)
    {
        return command.Answers.Select(a => new UserAnswer
        {
            QuizQuestionId = a.QuizQuestionId,
            AnswerText = a.AnswerText,
            AnsweredAt = DateTime.UtcNow
        });
    }
}