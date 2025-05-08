using Moq;
using QuizService.Application.Grpc;
using QuizService.Application.Services;
using Shared.Question.Grpc.Proto;
using Xunit;

namespace QuizService.Application.Tests.Services;

public class QuestionValidationServiceTests
{
    private readonly Mock<IQuestionGrpcClient> _questionGrpcClientMock;
    private readonly QuestionValidationService _sut;

    public QuestionValidationServiceTests()
    {
        _questionGrpcClientMock = new Mock<IQuestionGrpcClient>();
        _sut = new QuestionValidationService(_questionGrpcClientMock.Object);
    }

    [Fact]
    public async Task ValidateQuestionsExistAsync_WhenAllQuestionsExist_ShouldNotThrowException()
    {
        // Arrange
        var questionIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var response = new GetQuestionsResponse
        {
            Questions =
            {
                new Question
                {
                    Id = questionIds[0].ToString()
                },
                new Question
                {
                    Id = questionIds[1].ToString()
                }
            }
        };

        _questionGrpcClientMock
            .Setup(x => x.GetQuestionsByIdsAsync(
                It.Is<GetQuestionsRequest>(r => 
                    r.QuestionIds.Count == questionIds.Length &&
                    r.QuestionIds.All(id => questionIds.Contains(Guid.Parse(id)))),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act & Assert
        await _sut.ValidateQuestionsExistAsync(questionIds);
    }

    [Fact]
    public async Task ValidateQuestionsExistAsync_WhenSomeQuestionsDoNotExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var questionIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var response = new GetQuestionsResponse
        {
            Questions =
            {
                new Question
                {
                    Id = questionIds[0].ToString()
                }
            }
        };

        _questionGrpcClientMock
            .Setup(x => x.GetQuestionsByIdsAsync(
                It.Is<GetQuestionsRequest>(r => 
                    r.QuestionIds.Count == questionIds.Length &&
                    r.QuestionIds.All(id => questionIds.Contains(Guid.Parse(id)))),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.ValidateQuestionsExistAsync(questionIds));
        
        Assert.Contains(questionIds[1].ToString(), exception.Message);
    }

    [Fact]
    public async Task ValidateQuestionsExistAsync_WhenNoQuestionsExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var questionIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var response = new GetQuestionsResponse();

        _questionGrpcClientMock
            .Setup(x => x.GetQuestionsByIdsAsync(
                It.Is<GetQuestionsRequest>(r => 
                    r.QuestionIds.Count == questionIds.Length &&
                    r.QuestionIds.All(id => questionIds.Contains(Guid.Parse(id)))),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.ValidateQuestionsExistAsync(questionIds));
        
        Assert.Contains(questionIds[0].ToString(), exception.Message);
        Assert.Contains(questionIds[1].ToString(), exception.Message);
    }

    [Fact]
    public async Task ValidateQuestionsExistAsync_WhenGrpcClientThrowsException_ShouldPropagateException()
    {
        // Arrange
        var questionIds = new[] { Guid.NewGuid() };
        var expectedException = new Exception("GRPC error");

        _questionGrpcClientMock
            .Setup(x => x.GetQuestionsByIdsAsync(
                It.Is<GetQuestionsRequest>(r => 
                    r.QuestionIds.Count == questionIds.Length &&
                    r.QuestionIds.All(id => questionIds.Contains(Guid.Parse(id)))),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<Exception>(
            () => _sut.ValidateQuestionsExistAsync(questionIds));
        
        Assert.Same(expectedException, actualException);
    }

    [Fact]
    public async Task ValidateQuestionsExistAsync_WhenQuestionIdsIsEmpty_ShouldNotThrowException()
    {
        // Arrange
        var questionIds = Array.Empty<Guid>();
        var response = new GetQuestionsResponse();

        _questionGrpcClientMock
            .Setup(x => x.GetQuestionsByIdsAsync(
                It.Is<GetQuestionsRequest>(r => r.QuestionIds.Count == 0),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act & Assert
        await _sut.ValidateQuestionsExistAsync(questionIds);
    }

    [Fact]
    public async Task ValidateQuestionsExistAsync_WhenCancellationTokenIsCancelled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var questionIds = new[] { Guid.NewGuid() };
        var cancellationToken = new CancellationToken(true);

        _questionGrpcClientMock
            .Setup(x => x.GetQuestionsByIdsAsync(
                It.Is<GetQuestionsRequest>(r => 
                    r.QuestionIds.Count == questionIds.Length &&
                    r.QuestionIds.All(id => questionIds.Contains(Guid.Parse(id)))),
                It.Is<CancellationToken>(ct => ct.IsCancellationRequested)))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _sut.ValidateQuestionsExistAsync(questionIds, cancellationToken));
    }
} 