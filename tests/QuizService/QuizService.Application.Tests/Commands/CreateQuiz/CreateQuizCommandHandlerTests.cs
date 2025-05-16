using Moq;
using QuizService.Application.Commands.CreateQuiz;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Repositories;

namespace QuizService.Application.Tests.Commands.CreateQuiz;

public class CreateQuizCommandHandlerTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly Mock<IQuestionService> _questionValidationServiceMock;
    private readonly CreateQuizCommandHandler _handler;

    public CreateQuizCommandHandlerTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _questionValidationServiceMock = new Mock<IQuestionService>();
        _handler = new CreateQuizCommandHandler(
            _quizRepositoryMock.Object,
            _questionValidationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateQuizWithQuestions_AndReturnQuizId()
    {
        // Arrange
        var command = new CreateQuizCommand(new CreateQuizDto
        {
            Title = "Test Quiz",
            Description = "Test Description",
            Questions = new List<QuizQuestionDto>
            {
                new() { QuestionId = Guid.NewGuid(), Order = 1 },
                new() { QuestionId = Guid.NewGuid(), Order = 2 }
            }
        });

        _questionValidationServiceMock
            .Setup(x => x.ValidateQuestionsExistAsync(
                It.Is<IEnumerable<Guid>>(ids => 
                    ids.Count() == command.Quiz.Questions.Count &&
                    ids.All(id => command.Quiz.Questions.Any(q => q.QuestionId == id))),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        Quiz? savedQuiz = null;
        _quizRepositoryMock.Setup(x => x.InsertAsync(
            It.Is<Quiz>(q => 
                q.Title == command.Quiz.Title && 
                q.Description == command.Quiz.Description &&
                q.Questions.Count == command.Quiz.Questions.Count &&
                q.Questions.All(qq => command.Quiz.Questions.Any(cq => 
                    qq.QuestionId == cq.QuestionId && 
                    qq.Order == cq.Order))),
            It.Is<bool>(autoSave => autoSave == true),
            default))
            .Callback<Quiz, bool, CancellationToken>((quiz, _, _) => savedQuiz = quiz)
            .ReturnsAsync((Quiz quiz, bool _, CancellationToken _) => quiz);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        
        _questionValidationServiceMock.Verify(x => x.ValidateQuestionsExistAsync(
            It.Is<IEnumerable<Guid>>(ids => 
                ids.Count() == command.Quiz.Questions.Count &&
                ids.All(id => command.Quiz.Questions.Any(q => q.QuestionId == id))),
            It.IsAny<CancellationToken>()),
            Times.Once);
        
        _quizRepositoryMock.Verify(x => x.InsertAsync(
            It.Is<Quiz>(q => 
                q.Title == command.Quiz.Title && 
                q.Description == command.Quiz.Description &&
                q.Questions.Count == command.Quiz.Questions.Count &&
                q.Questions.All(qq => command.Quiz.Questions.Any(cq => 
                    qq.QuestionId == cq.QuestionId && 
                    qq.Order == cq.Order))),
            It.Is<bool>(autoSave => autoSave == true),
            default), 
            Times.Once);
        
        Assert.NotNull(savedQuiz);
        Assert.Equal(command.Quiz.Title, savedQuiz!.Title);
        Assert.Equal(command.Quiz.Description, savedQuiz.Description);
        Assert.Equal(command.Quiz.Questions.Count, savedQuiz.Questions.Count);
        
        foreach (var (expected, actual) in command.Quiz.Questions.Zip(savedQuiz.Questions.OrderBy(q => q.Order)))
        {
            Assert.Equal(expected.QuestionId, actual.QuestionId);
            Assert.Equal(expected.Order, actual.Order);
        }
    }

    [Fact]
    public async Task Handle_WhenQuestionsDoNotExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateQuizCommand(new CreateQuizDto
        {
            Title = "Test Quiz",
            Description = "Test Description",
            Questions = new List<QuizQuestionDto>
            {
                new() { QuestionId = Guid.NewGuid(), Order = 1 },
                new() { QuestionId = Guid.NewGuid(), Order = 2 }
            }
        });

        _questionValidationServiceMock
            .Setup(x => x.ValidateQuestionsExistAsync(
                It.Is<IEnumerable<Guid>>(ids => 
                    ids.Count() == command.Quiz.Questions.Count &&
                    ids.All(id => command.Quiz.Questions.Any(q => q.QuestionId == id))),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Some questions do not exist"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, default));
        
        Assert.Equal("Some questions do not exist", exception.Message);
        
        _quizRepositoryMock.Verify(x => x.InsertAsync(
            It.IsAny<Quiz>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidationServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new CreateQuizCommand(new CreateQuizDto
        {
            Title = "Test Quiz",
            Description = "Test Description",
            Questions = new List<QuizQuestionDto>
            {
                new() { QuestionId = Guid.NewGuid(), Order = 1 }
            }
        });

        var expectedException = new Exception("Validation service error");
        _questionValidationServiceMock
            .Setup(x => x.ValidateQuestionsExistAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, default));
        
        Assert.Same(expectedException, actualException);
        
        _quizRepositoryMock.Verify(x => x.InsertAsync(
            It.IsAny<Quiz>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }
} 