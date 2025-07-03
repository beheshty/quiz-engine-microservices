using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using QuizService.Domain.Enums;
using BuildingBlocks.Domain.Exceptions;

namespace QuizService.Application.Commands.UpdateQuiz;

public class UpdateQuizCommandHandler : ICommandHandler<UpdateQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionService _questionService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuizCommandHandler(
        IQuizRepository quizRepository,
        IQuestionService questionValidationService,
        IUnitOfWork unitOfWork)
    {
        _quizRepository = quizRepository;
        _questionService = questionValidationService;
        _unitOfWork = unitOfWork;
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
        var questionIds = command.Quiz.Questions.Select(q => q.QuestionId);
        await _questionService.ValidateQuestionsExistAsync(questionIds, cancellationToken);

        // Update basic properties
        quiz.Title = command.Quiz.Title;
        quiz.Description = command.Quiz.Description;

        // Clear existing questions and add new ones
        quiz.Questions.Clear();
        var questions = command.Quiz.Questions.Select(q => new QuizQuestion
        {
            QuestionId = q.QuestionId,
            Order = q.Order
        });
        quiz.AddQuestions(questions);

        await _quizRepository.UpdateAsync(quiz, cancellationToken: cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return quiz.Id;
    }
} 