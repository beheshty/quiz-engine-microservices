using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Enums;

namespace QuizService.Domain.Tests.Entities.QuizManagement;

public class QuizTests
{
    [Fact]
    public void Constructor_ShouldInitializeQuiz_WithProvidedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Quiz";
        var description = "Test Description";

        // Act
        var quiz = new Quiz(id, title, description);

        // Assert
        Assert.Equal(id, quiz.Id);
        Assert.Equal(title, quiz.Title);
        Assert.Equal(description, quiz.Description);
        Assert.Equal(QuizStatus.Draft, quiz.Status);
        Assert.Empty(quiz.Questions);
    }

    [Fact]
    public void AddQuestions_ShouldAddQuestionsToCollection()
    {
        // Arrange
        var quiz = new Quiz(Guid.NewGuid(), "Test Quiz", "Test Description");
        var questions = new List<QuizQuestion>
        {
            new() { QuestionId = Guid.NewGuid(), Order = 1 },
            new() { QuestionId = Guid.NewGuid(), Order = 2 }
        };

        // Act
        quiz.AddQuestions(questions);

        // Assert
        Assert.Equal(questions.Count, quiz.Questions.Count);
        Assert.Equal(questions.OrderBy(q => q.Order), quiz.Questions.OrderBy(q => q.Order), 
            new QuizQuestionComparer());
    }

    [Theory]
    [InlineData(QuizStatus.Draft, QuizStatus.Published)]
    [InlineData(QuizStatus.Published, QuizStatus.Archived)]
    public void ChangeStatus_ShouldUpdateStatus_WhenValidTransition(QuizStatus initialStatus, QuizStatus newStatus)
    {
        // Arrange
        var quiz = new Quiz(Guid.NewGuid(), "Test Quiz", "Test Description");
        if (initialStatus != QuizStatus.Draft)
        {
            quiz.ChangeStatus(QuizStatus.Published); // Move to Published if needed
        }
        if (initialStatus == QuizStatus.Archived)
        {
            quiz.ChangeStatus(QuizStatus.Archived); // Move to Archived if needed
        }

        // Act
        quiz.ChangeStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, quiz.Status);
    }

    [Theory]
    [InlineData(QuizStatus.Published)]
    [InlineData(QuizStatus.Archived)]
    public void ChangeStatus_ShouldThrowException_WhenChangingToDraft(QuizStatus initialStatus)
    {
        // Arrange
        var quiz = new Quiz(Guid.NewGuid(), "Test Quiz", "Test Description");
        quiz.ChangeStatus(initialStatus);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => quiz.ChangeStatus(QuizStatus.Draft));
        Assert.Equal("Cannot change status to Draft", exception.Message);
    }

    private class QuizQuestionComparer : IEqualityComparer<QuizQuestion>
    {
        public bool Equals(QuizQuestion? x, QuizQuestion? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.QuestionId == y.QuestionId && x.Order == y.Order;
        }

        public int GetHashCode(QuizQuestion obj)
        {
            return HashCode.Combine(obj.QuestionId, obj.Order);
        }
    }
} 