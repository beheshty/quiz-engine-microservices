using Moq;
using QuizService.Application.Commands.UpdateQuiz;
using QuizService.Application.Services;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Shared.Domain.Exceptions;
using Xunit;

namespace QuizService.Application.Tests.Commands.UpdateQuiz;

public class UpdateQuizCommandHandlerTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly Mock<IQuestionValidationService> _questionValidationServiceMock;
    private readonly UpdateQuizCommandHandler _handler;

    public UpdateQuizCommandHandlerTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _questionValidationServiceMock = new Mock<IQuestionValidationService>();
        _handler = new UpdateQuizCommandHandler(_quizRepositoryMock.Object, _questionValidationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenQuizNotInDraftStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var command = new UpdateQuizCommand
        {
            Id = quizId,
            Title = "Updated Title",
            Description = "Updated Description",
            Questions = new List<QuizQuestionDto>()
        };

        var quiz = new Quiz(quizId, "Original Title", "Original Description");
        quiz.ChangeStatus(QuizStatus.Published);

        _quizRepositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(id => id == quizId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command));
        Assert.Contains("Only quizzes in Draft status can be updated", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenQuestionsDoNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var command = new UpdateQuizCommand
        {
            Id = quizId,
            Title = "Updated Title",
            Description = "Updated Description",
            Questions = new List<QuizQuestionDto>
            {
                new() { QuestionId = questionId, Order = 1 }
            }
        };

        var quiz = new Quiz(quizId, "Original Title", "Original Description");

        _quizRepositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(id => id == quizId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        _questionValidationServiceMock.Setup(x => x.ValidateQuestionsExistAsync(
                It.Is<IEnumerable<Guid>>(ids => ids.Single() == questionId), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Questions do not exist"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command));
    }

    [Fact]
    public async Task Handle_WhenValidCommand_UpdatesQuizSuccessfully()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var command = new UpdateQuizCommand
        {
            Id = quizId,
            Title = "Updated Title",
            Description = "Updated Description",
            Questions = new List<QuizQuestionDto>
            {
                new() { QuestionId = questionId, Order = 1 }
            }
        };

        var quiz = new Quiz(quizId, "Original Title", "Original Description");

        _quizRepositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(id => id == quizId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        _questionValidationServiceMock.Setup(x => x.ValidateQuestionsExistAsync(
                It.Is<IEnumerable<Guid>>(ids => ids.Single() == questionId), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _quizRepositoryMock.Setup(x => x.UpdateAsync(
                It.Is<Quiz>(q => 
                    q.Id == quizId && 
                    q.Title == command.Title && 
                    q.Description == command.Description && 
                    q.Questions.Single().QuestionId == questionId && 
                    q.Questions.Single().Order == 1), 
                true, 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.Equal(quizId, result);
        Assert.Equal(command.Title, quiz.Title);
        Assert.Equal(command.Description, quiz.Description);
        Assert.Single(quiz.Questions);
        Assert.Equal(questionId, quiz.Questions.First().QuestionId);
        Assert.Equal(1, quiz.Questions.First().Order);

        _quizRepositoryMock.Verify(x => x.UpdateAsync(
            It.Is<Quiz>(q => 
                q.Id == quizId && 
                q.Title == command.Title && 
                q.Description == command.Description && 
                q.Questions.Single().QuestionId == questionId && 
                q.Questions.Single().Order == 1), 
            true, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }
} 