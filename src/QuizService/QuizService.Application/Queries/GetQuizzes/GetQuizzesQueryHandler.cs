using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Repositories;
using QuizService.Domain.Filters;

namespace QuizService.Application.Queries.GetQuizzes;

public class GetQuizzesQueryHandler : IQueryHandler<GetQuizzesQuery, List<QuizListItemDto>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizzesQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<List<QuizListItemDto>> Handle(GetQuizzesQuery query, CancellationToken cancellationToken = default)
    {
        var filter = new QuizFilterDto
        {
            Title = query.Title,
            Status = query.Status,
            CreatedFrom = query.CreatedFrom,
            CreatedTo = query.CreatedTo
        };

        var quizzes = await _quizRepository.GetListAsync(filter, cancellationToken);
        return [.. quizzes.Select(q => new QuizListItemDto
        {
            Id = q.Id,
            Title = q.Title,
            Description = q.Description,
            Status = q.Status,
            CreatedAt = q.CreationTime
        })];
    }
} 