using QuestionService.Application.DTOs;
using QuestionService.Domain.Entities;
using QuestionService.Domain.Repositories;

namespace QuestionService.Application.Services;

public class QuestionAppService : IQuestionAppService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionAppService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<QuestionDto> CreateQuestionAsync(CreateQuestionDto createQuestionDto, CancellationToken cancellationToken = default)
    {
        var question = new Question(
            createQuestionDto.Text,
            createQuestionDto.DifficultyLevel);

        question.AddOptions(createQuestionDto.Options.Select(o => 
        new Option(
            o.Content,
            o.IsCorrect,
            o.Order)).ToArray());

        await _questionRepository.InsertAsync(question, cancellationToken: cancellationToken);

        return MapToDto(question);
    }

    public async Task<QuestionDto> UpdateQuestionAsync(Guid id, UpdateQuestionDto updateQuestionDto, CancellationToken cancellationToken = default)
    {
        var question = await _questionRepository.GetAsync(id, cancellationToken);

        question.Update(updateQuestionDto.Text, updateQuestionDto.DifficultyLevel);

        // Clear existing options and add new ones
        question.Options.Clear();
        question.AddOptions(updateQuestionDto.Options.Select(o =>
         new Option(
             o.Content,
             o.IsCorrect,
             o.Order)).ToArray());

        await _questionRepository.UpdateAsync(question, cancellationToken: cancellationToken);

        return MapToDto(question);
    }

    public async Task DeleteQuestionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var question = await _questionRepository.GetAsync(id, cancellationToken);
        //TODO: A validation will be add in which If the question is used in any Quiz, it cannot be deleted
        await _questionRepository.DeleteAsync(question, cancellationToken: cancellationToken);
    }

    public async Task<QuestionDto> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var question = await _questionRepository.GetAsync(id, cancellationToken);
      
        return MapToDto(question);
    }

    public async Task<List<QuestionDto>> GetQuestionByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        var questions = await _questionRepository.GetByIdsAsync(ids, cancellationToken);
      
        return [.. questions.Select(MapToDto)];
    }

    public async Task<PaginatedResultDto<QuestionDto>> GetFilteredQuestionsAsync(QuestionFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Get filtered and paginated results
        var result = await _questionRepository.GetFilteredListAsync(
            searchText: filter.SearchText,
            difficultyLevel: filter.DifficultyLevel,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            cancellationToken: cancellationToken);

        var items = result.Items;
        var totalCount = result.TotalCount;

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        // Map to DTOs
        var questionDtos = items.Select(MapToDto).ToList();

        // Return paginated result
        return new PaginatedResultDto<QuestionDto>
        {
            Items = questionDtos,
            TotalCount = totalCount
        };
    }

    private static QuestionDto MapToDto(Question question)
    {
        return new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            DifficultyLevel = question.DifficultyLevel,
            Options = [.. question.Options.Select(o => new OptionDto
            {
                Id = o.Id,
                Content = o.Content,
                IsCorrect = o.IsCorrect,
                Order = o.Order
            })],
            CreationTime = question.CreationTime,
            LastModificationTime = question.LastModificationTime
        };
    }
}