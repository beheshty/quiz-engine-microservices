using Moq;
using QuizService.Application.Commands.ProcessUserQuiz;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Shared.Domain.Exceptions;
using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Tests.Commands.ProcessUserQuiz;

public class ProcessUserQuizCommandHandlerTests
{
    private readonly Mock<IUserQuizRepository> _userQuizRepositoryMock;
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly ProcessUserQuizCommandHandler _handler;

    public ProcessUserQuizCommandHandlerTests()
    {
        _userQuizRepositoryMock = new Mock<IUserQuizRepository>();
        _questionServiceMock = new Mock<IQuestionService>();
        _handler = new ProcessUserQuizCommandHandler(
            _userQuizRepositoryMock.Object,
            _questionServiceMock.Object);
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
        var answers = new List<UserQuizAnswerDto>
        {
            new UserQuizAnswerDto { QuizQuestionId = Guid.NewGuid(), AnswerText = "A" },
            new UserQuizAnswerDto { QuizQuestionId = Guid.NewGuid(), AnswerText = "B" }
        };
        var command = new ProcessUserQuizCommand(userQuizId, userId, answers);
        _userQuizRepositoryMock.Setup(x => x.GetAsync(userQuizId, default)).ReturnsAsync(userQuiz);
        _questionServiceMock.Setup(x => x.GetQuestionsByIdsAsync(It.IsAny<IEnumerable<Guid>>(), default))
            .ReturnsAsync(new GetQuestionsResponse
            {
                Questions =
                {
                    new Question { Id = answers[0].QuizQuestionId.ToString(), CorrectAnswerOptionId = answers[0].QuizQuestionId.ToString() },
                    new Question { Id = answers[1].QuizQuestionId.ToString(), CorrectAnswerOptionId = answers[1].QuizQuestionId.ToString() }
                }
            });
        _userQuizRepositoryMock.Setup(x => x.UpdateAsync(userQuiz, true, default)).ReturnsAsync(userQuiz);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _userQuizRepositoryMock.Verify(x => x.UpdateAsync(userQuiz, true, default), Times.Once);
        Assert.Equal(userQuizId, result.UserQuizId);
        Assert.Equal(userQuiz.Status, result.Status);
    }
} 