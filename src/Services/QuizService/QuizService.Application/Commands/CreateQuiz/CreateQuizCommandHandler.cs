using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;

namespace QuizService.Application.Commands.CreateQuiz;

public class CreateQuizCommandHandler : ICommandHandler<CreateQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionService _questionService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuizCommandHandler(
        IQuizRepository quizRepository,
        IQuestionService questionValidationService,
        IUnitOfWork unitOfWork)
    {
        _quizRepository = quizRepository;
        _questionService = questionValidationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateQuizCommand command, CancellationToken cancellationToken = default)
    {
        // Validate that all questions exist
        var questionIds = command.Quiz.Questions.Select(q => q.QuestionId);
        await _questionService.ValidateQuestionsExistAsync(questionIds, cancellationToken);

        var quiz = new Quiz(Guid.NewGuid(), command.Quiz.Title, command.Quiz.Description);

        var questions = command.Quiz.Questions.Select(q => new QuizQuestion
        {
            QuestionId = q.QuestionId,
            Order = q.Order
        });

        quiz.AddQuestions(questions);

        await _quizRepository.InsertAsync(quiz, cancellationToken: cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return quiz.Id;
    }
} 