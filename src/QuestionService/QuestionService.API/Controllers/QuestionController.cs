using Microsoft.AspNetCore.Mvc;
using QuestionService.Application.DTOs;
using QuestionService.Application.Services;

namespace QuestionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionAppService _questionAppService;

    public QuestionController(IQuestionAppService questionAppService)
    {
        _questionAppService = questionAppService;
    }

    /// <summary>
    /// Creates a new question
    /// </summary>
    /// <param name="createQuestionDto">The question data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created question</returns>
    [HttpPost]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionDto>> CreateQuestion(
        [FromBody] CreateQuestionDto createQuestionDto,
        CancellationToken cancellationToken = default)
    {
        var question = await _questionAppService.CreateQuestionAsync(createQuestionDto, cancellationToken);
        return CreatedAtAction(nameof(GetQuestionById), new { id = question.Id }, question);
    }

    /// <summary>
    /// Updates an existing question
    /// </summary>
    /// <param name="id">The question ID</param>
    /// <param name="updateQuestionDto">The updated question data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated question</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionDto>> UpdateQuestion(
        [FromRoute] Guid id,
        [FromBody] UpdateQuestionDto updateQuestionDto,
        CancellationToken cancellationToken = default)
    {
        var question = await _questionAppService.UpdateQuestionAsync(id, updateQuestionDto, cancellationToken);
        return Ok(question);
    }

    /// <summary>
    /// Deletes a question
    /// </summary>
    /// <param name="id">The question ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestion(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _questionAppService.DeleteQuestionAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Gets a question by ID
    /// </summary>
    /// <param name="id">The question ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The question</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionDto>> GetQuestionById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var question = await _questionAppService.GetQuestionByIdAsync(id, cancellationToken);
        return Ok(question);
    }

    /// <summary>
    /// Gets filtered questions with pagination
    /// </summary>
    /// <param name="filter">The filter criteria</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of questions</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResultDto<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResultDto<QuestionDto>>> GetFilteredQuestions(
        [FromQuery] QuestionFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _questionAppService.GetFilteredQuestionsAsync(filter, cancellationToken);
        return Ok(result);
    }
} 