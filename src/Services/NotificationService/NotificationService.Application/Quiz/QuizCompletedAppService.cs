using NotificationService.Application.DTOs;
using NotificationService.Application.Grpc;
using NotificationService.Infrastructure.Abstractions;

namespace NotificationService.Application.Quiz
{
    public class QuizCompletedAppService
    {
        private readonly IUserGrpcClient _userGrpcClient;
        private readonly IEmailSender _emailSender;

        public QuizCompletedAppService(IUserGrpcClient userGrpcClient, IEmailSender emailSender)
        {
            _userGrpcClient = userGrpcClient;
            _emailSender = emailSender;
        }

        public async Task NotifyQuizCompletionAsync(UserQuizDto userQuizDto, CancellationToken cancellationToken)
        {
            var userInfo = await _userGrpcClient.GetUserInfoAsync(userQuizDto.UserId, cancellationToken);
            if (userInfo == null)
            {
                throw new ArgumentException($"User with ID {userQuizDto.UserId} not found.");
            }
            //Temp approach: the mail text should be moved to a message template provider service
            await _emailSender.SendEmailAsync(userInfo.Email, "Quiz Completed",
                $"Congratulations {userInfo.FirstName}, you have completed the quiz '{userQuizDto.QuizId}'. " +
                $"Your score is {userQuizDto.TotalScore} with {userQuizDto.CorrectCount} correct answers and {userQuizDto.WrongCount} wrong answers. " +
                $"You took {userQuizDto.TimeTaken?.TotalMinutes} minutes to complete the quiz.", cancellationToken: cancellationToken);
        }

    }
}
