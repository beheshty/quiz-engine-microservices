using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuizService.Application.Commands.CreateQuiz;

public class CreateQuizCommandHandler : ICommandHandler<CreateQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionValidationService _questionValidationService;

    public CreateQuizCommandHandler(
        IQuizRepository quizRepository,
        IQuestionValidationService questionValidationService)
    {
        _quizRepository = quizRepository;
        _questionValidationService = questionValidationService;
    }

    public async Task<Guid> Handle(CreateQuizCommand command, CancellationToken cancellationToken = default)
    {
        // Validate that all questions exist
        var questionIds = command.Quiz.Questions.Select(q => q.QuestionId);
        await _questionValidationService.ValidateQuestionsExistAsync(questionIds, cancellationToken);

        var quizId = Guid.NewGuid();
        var quiz = new Quiz(quizId, command.Quiz.Title, command.Quiz.Description);

        var questions = command.Quiz.Questions.Select(q => new QuizQuestion
        {
            QuizId = quizId,
            QuestionId = q.QuestionId,
            Order = q.Order
        });

        quiz.AddQuestions(questions);

        await _quizRepository.InsertAsync(quiz, true, cancellationToken);

        return quizId;
    }
} 