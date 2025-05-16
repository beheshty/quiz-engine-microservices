using Moq;
using QuizService.Application.Commands.DraftUserQuiz;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Shared.Domain.Exceptions;

namespace QuizService.Application.Tests.Commands.DraftUserQuiz;

public class DraftUserQuizCommandHandlerTests
{
    private readonly Mock<IUserQuizRepository> _userQuizRepositoryMock;
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly DraftUserQuizCommandHandler _handler;

    public DraftUserQuizCommandHandlerTests()
    {
        _userQuizRepositoryMock = new Mock<IUserQuizRepository>();
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _handler = new DraftUserQuizCommandHandler(
            _userQuizRepositoryMock.Object,
            _quizRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserQuizAndReturnQuizDetails()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quiz = new Quiz(quizId, "Sample Quiz", "Description");
        quiz.ChangeStatus(QuizStatus.Published);
        quiz.AddQuestions(new List<QuizQuestion>
        {
            new QuizQuestion { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Order = 1 },
            new QuizQuestion { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Order = 2 }
        });
        _quizRepositoryMock.Setup(x => x.GetAsync(quizId, default)).ReturnsAsync(quiz);
        UserQuiz? savedUserQuiz = null;
        _userQuizRepositoryMock.Setup(x => x.InsertAsync(
            It.Is<UserQuiz>(uq => uq.UserId == userId && uq.QuizId == quizId),
            true,
            default)).Callback<UserQuiz, bool, CancellationToken>((uq, _, _) => savedUserQuiz = uq);

        var command = new DraftUserQuizCommand(userId, quizId);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(quizId, result.QuizId);
        Assert.Equal("Sample Quiz", result.Title);
        Assert.Equal("Description", result.Description);
        Assert.Equal(QuizStatus.Published, result.Status);
        Assert.Equal(2, result.Questions.Count);
        Assert.All(result.Questions, q => Assert.NotEqual(Guid.Empty, q.QuizQuestionId));
        Assert.NotNull(savedUserQuiz);
        Assert.Equal(userId, savedUserQuiz!.UserId);
        Assert.Equal(quizId, savedUserQuiz.QuizId);
        Assert.Equal(UserQuizStatus.NotStarted, savedUserQuiz.Status);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenQuizNotPublished()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quiz = new Quiz(quizId, "Sample Quiz", "Description");
        _quizRepositoryMock.Setup(x => x.GetAsync(quizId, default)).ReturnsAsync(quiz);
        var command = new DraftUserQuizCommand(userId, quizId);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
} 