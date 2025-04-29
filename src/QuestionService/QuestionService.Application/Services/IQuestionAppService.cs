using QuestionService.Application.DTOs;

namespace QuestionService.Application.Services;

public interface IQuestionAppService
{
    Task<QuestionDto> CreateQuestionAsync(CreateQuestionDto createQuestionDto, CancellationToken cancellationToken = default);
    Task<QuestionDto> UpdateQuestionAsync(Guid id, UpdateQuestionDto updateQuestionDto, CancellationToken cancellationToken = default);
    Task DeleteQuestionAsync(Guid id, CancellationToken cancellationToken = default);
    Task<QuestionDto> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResultDto<QuestionDto>> GetFilteredQuestionsAsync(QuestionFilterDto filter, CancellationToken cancellationToken = default);
} 