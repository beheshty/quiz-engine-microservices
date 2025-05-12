using Moq;
using QuizService.Application.Queries.GetQuizzes;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Enums;
using QuizService.Domain.Filters;
using QuizService.Domain.Repositories;
using Xunit;

namespace QuizService.Application.Tests.Queries.GetQuizzes;

public class GetQuizzesQueryHandlerTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly GetQuizzesQueryHandler _handler;

    public GetQuizzesQueryHandlerTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _handler = new GetQuizzesQueryHandler(_quizRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldMapFiltersAndReturnQuizListItems()
    {
        // Arrange
        var query = new GetQuizzesQuery(
            Title: "Test",
            Status: QuizStatus.Draft,
            CreatedFrom: DateTime.UtcNow.AddDays(-1),
            CreatedTo: DateTime.UtcNow
        );

        var quizzes = new List<Quiz>
        {
            new Quiz(Guid.NewGuid(), "Test Quiz 1", "Description 1"),
            new Quiz(Guid.NewGuid(), "Test Quiz 2", "Description 2")
        };

        _quizRepositoryMock.Setup(x => x.GetListAsync(
            It.Is<QuizFilterDto>(f =>
                f.Title == query.Title &&
                f.Status == query.Status &&
                f.CreatedFrom == query.CreatedFrom &&
                f.CreatedTo == query.CreatedTo),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(quizzes);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(quizzes.Count, result.Count);
        for (int i = 0; i < quizzes.Count; i++)
        {
            Assert.Equal(quizzes[i].Id, result[i].Id);
            Assert.Equal(quizzes[i].Title, result[i].Title);
            Assert.Equal(quizzes[i].Description, result[i].Description);
            Assert.Equal(quizzes[i].Status, result[i].Status);
            Assert.Equal(quizzes[i].CreationTime, result[i].CreatedAt);
        }
    }
} 