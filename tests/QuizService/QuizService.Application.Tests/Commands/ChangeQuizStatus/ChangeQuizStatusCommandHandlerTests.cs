using Moq;
using QuizService.Application.Commands.ChangeQuizStatus;
using QuizService.Domain.Entities;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;
using Xunit;

namespace QuizService.Application.Tests.Commands.ChangeQuizStatus;

public class ChangeQuizStatusCommandHandlerTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly ChangeQuizStatusCommandHandler _handler;

    public ChangeQuizStatusCommandHandlerTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _handler = new ChangeQuizStatusCommandHandler(_quizRepositoryMock.Object);
    }

    [Theory]
    [InlineData(QuizStatus.Draft, QuizStatus.Published)]
    [InlineData(QuizStatus.Published, QuizStatus.Archived)]
    public async Task Handle_WithDifferentStatusTransitions_ShouldChangeStatusCorrectly(QuizStatus initialStatus, QuizStatus newStatus)
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var quiz = new Quiz(quizId, "Test Quiz", "Test Description");
        quiz.ChangeStatus(initialStatus);
        
        _quizRepositoryMock.Setup(x => x.GetAsync(quizId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);
        
        _quizRepositoryMock.Setup(x => x.UpdateAsync(It.Is<Quiz>(q => q.Id == quizId && q.Status == newStatus), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quiz);

        var command = new ChangeQuizStatusCommand
        {
            QuizId = quizId,
            NewStatus = newStatus
        };

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.Equal(newStatus, result);
        Assert.Equal(newStatus, quiz.Status);
        _quizRepositoryMock.Verify(x => x.GetAsync(quizId, It.IsAny<CancellationToken>()), Times.Once);
        _quizRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Quiz>(q => q.Id == quizId && q.Status == newStatus), true, It.IsAny<CancellationToken>()), Times.Once);
    }
} 