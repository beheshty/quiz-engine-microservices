using System;

namespace QuizService.API.Models
{
    public class DraftUserQuizRequest
    {
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
    }
} 