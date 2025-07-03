using BuildingBlocks.EventBus.Abstraction.Distributed;
using Contracts.Events.QuizService.UserQuiz;
using NotificationService.Application.DTOs;
using NotificationService.Application.Quiz;

namespace NotificationService.Worker.Quizzes
{
    public class UserQuizCompletedEventHandler : IDistributedEventHandler<UserQuizCompletedIntegrationEvent>
    {
        private readonly IQuizCompletedAppService _quizCompletedAppService;

        public UserQuizCompletedEventHandler(IQuizCompletedAppService quizCompletedAppService)
        {
            _quizCompletedAppService = quizCompletedAppService;
        }

        public async Task HandleAsync(UserQuizCompletedIntegrationEvent input, CancellationToken cancellationToken = default)
        {
            await _quizCompletedAppService.NotifyQuizCompletionAsync(new UserQuizDto(
                                        input.UserId, 
                                        input.QuizId, 
                                        input.StartedAt,  
                                        input.CompletedAt, 
                                        input.TotalScore, 
                                        input.CorrectCount, 
                                        input.WrongCount,
                                        input.TimeTaken), cancellationToken);
        }
    }
}
