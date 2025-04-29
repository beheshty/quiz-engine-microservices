using Moq;
using QuestionService.Application.DTOs;
using QuestionService.Application.Services;
using QuestionService.Domain.Entities;
using QuestionService.Domain.Enums;
using QuestionService.Domain.Repositories;
using Xunit;

namespace QuestionService.Application.Tests.Services;

public class QuestionAppServiceTests
{
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly QuestionAppService _questionService;

    public QuestionAppServiceTests()
    {
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _questionService = new QuestionAppService(_mockQuestionRepository.Object);
    }

    [Fact]
    public async Task CreateQuestionAsync_ValidInput_ReturnsQuestionDto()
    {
        // Arrange
        var createQuestionDto = new CreateQuestionDto
        {
            Text = "Test Question",
            DifficultyLevel = DifficultyLevel.Easy,
            Options = new List<CreateOptionDto>
            {
                new() { Content = "Option 1", IsCorrect = true, Order = 1 },
                new() { Content = "Option 2", IsCorrect = false, Order = 2 }
            }
        };

        var expectedQuestion = new Question(createQuestionDto.Text, createQuestionDto.DifficultyLevel);
        expectedQuestion.AddOptions(
            new Option("Option 1", true, 1),
            new Option("Option 2", false, 2));

        _mockQuestionRepository
            .Setup(x => x.InsertAsync(
                It.Is<Question>(q => 
                    q.Text == createQuestionDto.Text && 
                    q.DifficultyLevel == createQuestionDto.DifficultyLevel &&
                    q.Options.Count == 2 &&
                    q.Options.Any(o => o.Content == "Option 1" && o.IsCorrect && o.Order == 1) &&
                    q.Options.Any(o => o.Content == "Option 2" && !o.IsCorrect && o.Order == 2)),
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedQuestion);

        // Act
        var result = await _questionService.CreateQuestionAsync(createQuestionDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createQuestionDto.Text, result.Text);
        Assert.Equal(createQuestionDto.DifficultyLevel, result.DifficultyLevel);
        Assert.Equal(2, result.Options.Count);
        
        _mockQuestionRepository.Verify(
            x => x.InsertAsync(
                It.Is<Question>(q => 
                    q.Text == createQuestionDto.Text && 
                    q.DifficultyLevel == createQuestionDto.DifficultyLevel),
                false,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateQuestionAsync_ValidInput_ReturnsUpdatedQuestionDto()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var existingQuestion = new Question("Old Question", DifficultyLevel.Easy);
        var updateQuestionDto = new UpdateQuestionDto
        {
            Text = "Updated Question",
            DifficultyLevel = DifficultyLevel.Medium,
            Options = new List<UpdateOptionDto>
            {
                new() { Content = "New Option 1", IsCorrect = true, Order = 1 },
                new() { Content = "New Option 2", IsCorrect = false, Order = 2 }
            }
        };

        _mockQuestionRepository
            .Setup(x => x.GetAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingQuestion);

        var updatedQuestion = new Question(updateQuestionDto.Text, updateQuestionDto.DifficultyLevel);
        updatedQuestion.AddOptions(
            new Option("New Option 1", true, 1),
            new Option("New Option 2", false, 2));

        _mockQuestionRepository
            .Setup(x => x.UpdateAsync(
                It.Is<Question>(q => 
                    q.Text == updateQuestionDto.Text && 
                    q.DifficultyLevel == updateQuestionDto.DifficultyLevel &&
                    q.Options.Count == 2 &&
                    q.Options.Any(o => o.Content == "New Option 1" && o.IsCorrect && o.Order == 1) &&
                    q.Options.Any(o => o.Content == "New Option 2" && !o.IsCorrect && o.Order == 2)),
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedQuestion);

        // Act
        var result = await _questionService.UpdateQuestionAsync(questionId, updateQuestionDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateQuestionDto.Text, result.Text);
        Assert.Equal(updateQuestionDto.DifficultyLevel, result.DifficultyLevel);
        Assert.Equal(2, result.Options.Count);

        _mockQuestionRepository.Verify(
            x => x.GetAsync(questionId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockQuestionRepository.Verify(
            x => x.UpdateAsync(
                It.Is<Question>(q => 
                    q.Text == updateQuestionDto.Text && 
                    q.DifficultyLevel == updateQuestionDto.DifficultyLevel),
                false,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteQuestionAsync_ValidId_DeletesQuestion()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var existingQuestion = new Question("Test Question", DifficultyLevel.Easy);

        _mockQuestionRepository
            .Setup(x => x.GetAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingQuestion);

        _mockQuestionRepository
            .Setup(x => x.DeleteAsync(
                It.Is<Question>(q => 
                    q.Text == existingQuestion.Text && 
                    q.DifficultyLevel == existingQuestion.DifficultyLevel),
                false,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _questionService.DeleteQuestionAsync(questionId);

        // Assert
        _mockQuestionRepository.Verify(
            x => x.GetAsync(questionId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockQuestionRepository.Verify(
            x => x.DeleteAsync(
                It.Is<Question>(q => 
                    q.Text == existingQuestion.Text && 
                    q.DifficultyLevel == existingQuestion.DifficultyLevel),
                false,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetQuestionByIdAsync_ValidId_ReturnsQuestionDto()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var existingQuestion = new Question("Test Question", DifficultyLevel.Easy);

        _mockQuestionRepository
            .Setup(x => x.GetAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingQuestion);

        // Act
        var result = await _questionService.GetQuestionByIdAsync(questionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingQuestion.Text, result.Text);
        Assert.Equal(existingQuestion.DifficultyLevel, result.DifficultyLevel);

        _mockQuestionRepository.Verify(
            x => x.GetAsync(questionId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetFilteredQuestionsAsync_ValidFilter_ReturnsPaginatedResult()
    {
        // Arrange
        var filter = new QuestionFilterDto
        {
            SearchText = "Test",
            DifficultyLevel = DifficultyLevel.Easy,
            PageNumber = 1,
            PageSize = 10
        };

        var questions = new List<Question>
        {
            new("Test Question 1", DifficultyLevel.Easy),
            new("Test Question 2", DifficultyLevel.Easy)
        };

        _mockQuestionRepository
            .Setup(x => x.GetFilteredListAsync(
                filter.SearchText,
                filter.DifficultyLevel,
                filter.PageNumber,
                filter.PageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((questions, 2));

        // Act
        var result = await _questionService.GetFilteredQuestionsAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.TotalCount);

        _mockQuestionRepository.Verify(
            x => x.GetFilteredListAsync(
                filter.SearchText,
                filter.DifficultyLevel,
                filter.PageNumber,
                filter.PageSize,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
} 