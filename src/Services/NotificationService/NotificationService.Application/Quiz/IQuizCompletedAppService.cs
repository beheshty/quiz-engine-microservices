
using NotificationService.Application.DTOs;

namespace NotificationService.Application.Quiz
{
    public interface IQuizCompletedAppService
    {
        Task NotifyQuizCompletionAsync(UserQuizDto userQuizDto, CancellationToken cancellationToken);
    }
}
