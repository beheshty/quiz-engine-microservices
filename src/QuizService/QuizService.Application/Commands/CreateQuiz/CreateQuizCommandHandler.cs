using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace QuizService.Application.Commands.CreateQuiz;

public class CreateQuizCommandHandler : ICommandHandler<CreateQuizCommand, Guid>
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuizCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<Guid> Handle(CreateQuizCommand command, CancellationToken cancellationToken = default)
    {
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