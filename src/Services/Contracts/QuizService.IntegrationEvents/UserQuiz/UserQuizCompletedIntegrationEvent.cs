using BuildingBlocks.EventBus.Abstraction.Distributed;
using System;

namespace Contracts.Events.QuizService.UserQuiz
{
    [DistributedMessage("user-quiz-completed")]
    public class UserQuizCompletedIntegrationEvent
    {
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; set; }
        public int? TotalScore { get; set; }
        public int? CorrectCount { get; set; }
        public int? WrongCount { get; set; }
        public TimeSpan? TimeTaken { get; set; }
    }
}
