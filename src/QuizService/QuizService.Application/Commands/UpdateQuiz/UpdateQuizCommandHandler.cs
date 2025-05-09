using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using QuizService.Domain.Enums;
using Shared.Domain.Exceptions;

namespace QuizService.Application.Commands.UpdateQuiz;

public class UpdateQuizCommandHandler : ICommandHandler<UpdateQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionValidationService _questionValidationService;

    public UpdateQuizCommandHandler(
        IQuizRepository quizRepository,
        IQuestionValidationService questionValidationService)
    {
        _quizRepository = quizRepository;
        _questionValidationService = questionValidationService;
    }

    public async Task<Guid> Handle(UpdateQuizCommand command, CancellationToken cancellationToken = default)
    {
        var quiz = await _quizRepository.GetAsync(command.Id, cancellationToken);
        if (quiz == null)
        {
            throw new EntityNotFoundException(typeof(Quiz), command.Id);
        }

        if (quiz.Status != QuizStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot update quiz with status {quiz.Status}. Only quizzes in Draft status can be updated.");
        }

        // Validate that all questions exist
        var questionIds = command.Questions.Select(q => q.QuestionId);
        await _questionValidationService.ValidateQuestionsExistAsync(questionIds, cancellationToken);

        // Update basic properties
        quiz.Title = command.Title;
        quiz.Description = command.Description;

        // Clear existing questions and add new ones
        quiz.Questions.Clear();
        var questions = command.Questions.Select(q => new QuizQuestion
        {
            QuestionId = q.QuestionId,
            Order = q.Order
        });
        quiz.AddQuestions(questions);

        await _quizRepository.UpdateAsync(quiz, true, cancellationToken);

        return quiz.Id;
    }
} 