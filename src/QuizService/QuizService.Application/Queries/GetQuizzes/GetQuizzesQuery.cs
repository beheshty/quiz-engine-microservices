using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Queries.GetQuizzes;
using QuizService.Domain.Enums;

namespace QuizService.Application.Queries.GetQuizzes;

public record GetQuizzesQuery(
    string? Title = null,
    QuizStatus? Status = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null
) : IQuery<List<QuizListItemDto>>; 