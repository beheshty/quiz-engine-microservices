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

    public ProcessUserQuizCommandHandler(
        IUserQuizRepository userQuizRepository,
        IQuestionService questionService)
    {
        _userQuizRepository = userQuizRepository;
        _questionService = questionService;
    }

    public async Task<ProcessUserQuizResultDto> Handle(ProcessUserQuizCommand command, CancellationToken cancellationToken = default)
    {
        var userQuiz = await _userQuizRepository.GetAsync(command.UserQuizId, cancellationToken);
        if (userQuiz == null || userQuiz.UserId != command.UserId)
            throw new EntityNotFoundException(typeof(UserQuiz), command.UserQuizId);
        if (userQuiz.Status == UserQuizStatus.Completed)
            throw new InvalidOperationException("Quiz already completed.");

        var questionIds = command.Answers.Select(a => a.QuizQuestionId);
        var questionResponse = await _questionService.GetQuestionsByIdsAsync(questionIds, cancellationToken);
        var correctAnswers = questionResponse.Questions.ToDictionary(
            q => Guid.Parse(q.Id),
            q => Guid.Parse(q.CorrectAnswerOptionId)
        );

        var userAnswers = command.Answers.Select(a => new UserAnswer
        {
            Id = Guid.NewGuid(),
            UserQuizId = userQuiz.Id,
            QuizQuestionId = a.QuizQuestionId,
            AnswerText = a.AnswerText,
            AnsweredAt = DateTime.UtcNow
        });

        userQuiz.ProcessAnswers(userAnswers, correctAnswers);
        await _userQuizRepository.UpdateAsync(userQuiz, true, cancellationToken);

        var result = new ProcessUserQuizResultDto
        {
            UserQuizId = userQuiz.Id,
            Status = userQuiz.Status,
        };

        return result;
    }
}