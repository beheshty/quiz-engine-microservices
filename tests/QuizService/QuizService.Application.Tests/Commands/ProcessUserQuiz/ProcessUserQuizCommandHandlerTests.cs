using BuildingBlocks.Domain.Exceptions;
using Moq;
using QuizService.Application.Commands.ProcessUserQuiz;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Contracts.Grpc.QuestionService.Proto;

namespace QuizService.Application.Tests.Commands.ProcessUserQuiz;

public class ProcessUserQuizCommandHandlerTests
{
    private readonly Mock<IUserQuizRepository> _userQuizRepositoryMock;
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProcessUserQuizCommandHandler _handler;

    public ProcessUserQuizCommandHandlerTests()
    {
        _userQuizRepositoryMock = new Mock<IUserQuizRepository>();
        _questionServiceMock = new Mock<IQuestionService>();
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new ProcessUserQuizCommandHandler(
            _userQuizRepositoryMock.Object,
            _questionServiceMock.Object,
            _quizRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenQuizAlreadyCompleted()
    {
        // Arrange
        var userQuizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var userQuiz = new UserQuiz(userQuizId, userId, Guid.NewGuid());
        typeof(UserQuiz).GetProperty("Status")!.SetValue(userQuiz, UserQuizStatus.Completed);
        var command = new ProcessUserQuizCommand(userQuizId, userId, new List<UserQuizAnswerDto>());
        _userQuizRepositoryMock.Setup(x => x.GetAsync(userQuizId, default)).ReturnsAsync(userQuiz);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserDoesNotOwnQuiz()
    {
        // Arrange
        var userQuizId = Guid.NewGuid();
        var userQuiz = new UserQuiz(userQuizId, Guid.NewGuid(), Guid.NewGuid());
        var command = new ProcessUserQuizCommand(userQuizId, Guid.NewGuid(), new List<UserQuizAnswerDto>());
        _userQuizRepositoryMock.Setup(x => x.GetAsync(userQuizId, default)).ReturnsAsync(userQuiz);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldProcessAnswersAndUpdateQuiz_WhenValid()
    {
        // Arrange
        var userQuizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quizId = Guid.NewGuid();
        var userQuiz = new UserQuiz(userQuizId, userId, quizId);
        var quiz = new Quiz(quizId, "Sample Quiz", "Description");
        quiz.AddQuestions(new List<QuizQuestion>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Order = 1,
                QuestionId = Guid.NewGuid(),
                QuizId = quiz.Id,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Order = 2,
                QuestionId = Guid.NewGuid(),
                QuizId = quiz.Id,
            },
        });
        var answers = new List<UserQuizAnswerDto>
        {
            new() { QuizQuestionId = quiz.Questions.First().Id, AnswerText = Guid.NewGuid().ToString() },
            new() { QuizQuestionId = quiz.Questions.Last().Id, AnswerText = Guid.NewGuid().ToString() }
        };
        var command = new ProcessUserQuizCommand(userQuizId, userId, answers);
        _userQuizRepositoryMock.Setup(x => x.GetAsync(userQuizId, default)).ReturnsAsync(userQuiz);
        _quizRepositoryMock.Setup(x => x.GetAsync(quizId, default)).ReturnsAsync(quiz);
        _questionServiceMock.Setup(x => x.GetQuestionsByIdsAsync(It.IsAny<IEnumerable<Guid>>(), default))
            .ReturnsAsync(new GetQuestionsResponse
            {
                Questions =
                {
                    new Question { Id = quiz.Questions.First().QuestionId.ToString(), CorrectAnswerOptionId = answers[0].AnswerText },
                    new Question { Id = quiz.Questions.Last().QuestionId.ToString(), CorrectAnswerOptionId = answers[1].AnswerText }
                }
            });
        _userQuizRepositoryMock.Setup(x => x.UpdateAsync(userQuiz, false, It.IsAny<CancellationToken>())).ReturnsAsync(userQuiz);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _userQuizRepositoryMock.Verify(x => x.UpdateAsync(userQuiz, false, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(userQuizId, result.UserQuizId);
        Assert.Equal(userQuiz.Status, result.Status);
    }
} 