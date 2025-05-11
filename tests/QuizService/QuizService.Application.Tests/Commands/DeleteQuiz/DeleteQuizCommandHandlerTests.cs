using Moq;
using QuizService.Application.Commands.DeleteQuiz;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Shared.Domain.Exceptions;
using Xunit;

namespace QuizService.Application.Tests.Commands.DeleteQuiz;

public class DeleteQuizCommandHandlerTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly DeleteQuizCommandHandler _handler;

    public DeleteQuizCommandHandlerTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _handler = new DeleteQuizCommandHandler(_quizRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenQuizNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var command = new DeleteQuizCommand(quizId);

        _quizRepositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(id => id == quizId), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Quiz)null!);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _handler.Handle(command));
    }

    [Fact]
    public async Task Handle_WhenQuizNotInDraftStatus_ThrowsInvalidOperationException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var command = new DeleteQuizCommand(quizId);

        var quiz = new Quiz(quizId, "Test Quiz", "Test Description");
        quiz.ChangeStatus(QuizStatus.Published);

        _quizRepositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(id => id == quizId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command));
        Assert.Contains("Only quizzes in Draft status can be deleted", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenQuizInDraftStatus_DeletesQuizSuccessfully()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var command = new DeleteQuizCommand(quizId);

        var quiz = new Quiz(quizId, "Test Quiz", "Test Description");
        // Quiz is in Draft status by default

        _quizRepositoryMock.Setup(x => x.GetAsync(It.Is<Guid>(id => id == quizId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        _quizRepositoryMock.Setup(x => x.DeleteAsync(
                It.Is<Quiz>(q => q.Id == quizId),
                true,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.Equal(quizId, result);
        
        _quizRepositoryMock.Verify(x => x.GetAsync(
            It.Is<Guid>(id => id == quizId),
            It.IsAny<CancellationToken>()),
            Times.Once);
            
        _quizRepositoryMock.Verify(x => x.DeleteAsync(
            It.Is<Quiz>(q => q.Id == quizId),
            true,
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
} 