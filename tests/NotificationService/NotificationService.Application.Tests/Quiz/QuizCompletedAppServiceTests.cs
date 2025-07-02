using Moq;
using NotificationService.Application.DTOs;
using NotificationService.Application.Grpc;
using NotificationService.Application.Quiz;
using NotificationService.Infrastructure.Abstractions;
using Shared.User.Grpc.Proto;
using Xunit;

namespace NotificationService.Application.Tests.Quiz
{
    public class QuizCompletedAppServiceTests
    {
        private readonly Mock<IUserGrpcClient> _mockUserGrpcClient;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly QuizCompletedAppService _quizCompletedAppService;

        public QuizCompletedAppServiceTests()
        {
            _mockUserGrpcClient = new Mock<IUserGrpcClient>();
            _mockEmailSender = new Mock<IEmailSender>();
            _quizCompletedAppService = new QuizCompletedAppService(
                _mockUserGrpcClient.Object,
                _mockEmailSender.Object);
        }

        [Fact]
        public async Task NotifyQuizCompletionAsync_ShouldSendEmail_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userQuizDto = new UserQuizDto(userId, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 90, 9, 1, TimeSpan.FromMinutes(10));

            var userInfo = new UserInfo
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Id = userId.ToString()
            };

            _mockUserGrpcClient.Setup(x => x.GetUserInfoAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            await _quizCompletedAppService.NotifyQuizCompletionAsync(userQuizDto, CancellationToken.None);

            // Assert
            _mockEmailSender.Verify(x => x.SendEmailAsync(
                userInfo.Email,
                "Quiz Completed",
                It.Is<string>(s => s.Contains($"Congratulations {userInfo.FirstName}, you have completed the quiz '{userQuizDto.QuizId}'.")), false,
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task NotifyQuizCompletionAsync_ShouldThrowArgumentException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userQuizDto = new UserQuizDto(userId, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 90, 9, 1, TimeSpan.FromMinutes(10));

            _mockUserGrpcClient.Setup(x => x.GetUserInfoAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserInfo)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _quizCompletedAppService.NotifyQuizCompletionAsync(userQuizDto, CancellationToken.None));

            Assert.Contains($"User with ID {userId} not found.", exception.Message);
            _mockEmailSender.Verify(x => x.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                false,
                It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}