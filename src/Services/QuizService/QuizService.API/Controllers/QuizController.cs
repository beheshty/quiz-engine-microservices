using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Commands.CreateQuiz;
using QuizService.Application.Commands.UpdateQuiz;
using QuizService.Application.Commands.ChangeQuizStatus;
using QuizService.Application.Commands.DeleteQuiz;
using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Queries.GetQuizzes;
using QuizService.Domain.Enums;
using QuizService.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace QuizService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuizController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public QuizController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Gets a list of quizzes with optional filters
    /// </summary>
    /// <param name="filters">Filter criteria for the quizzes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of quizzes matching the filters</returns>
    /// <response code="200">Returns the list of quizzes</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<QuizListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<QuizListItemDto>>> GetQuizzes(
        [FromQuery] QuizFilterRequest filters,
        CancellationToken cancellationToken = default)
    {
        var query = new GetQuizzesQuery(
            filters.Title,
            filters.Status,
            filters.CreatedFrom,
            filters.CreatedTo);
            
        var quizzes = await _dispatcher.Send(query, cancellationToken);
        
        return Ok(quizzes);
    }

    /// <summary>
    /// Creates a new quiz
    /// </summary>
    /// <param name="quiz">The quiz data to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created quiz</returns>
    /// <response code="201">Returns the newly created quiz ID</response>
    /// <response code="400">If the quiz data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<Guid>> CreateQuiz(CreateQuizDto quiz, CancellationToken cancellationToken = default)
    {
        var command = new CreateQuizCommand(quiz);
        var quizId = await _dispatcher.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(CreateQuiz), new { id = quizId }, quizId);
    }

    /// <summary>
    /// Updates an existing quiz
    /// </summary>
    /// <param name="id">The ID of the quiz to update</param>
    /// <param name="quiz">The updated quiz data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the updated quiz</returns>
    /// <response code="200">Returns the updated quiz ID</response>
    /// <response code="400">If the quiz data is invalid</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> UpdateQuiz(Guid id, UpdateQuizDto quiz, CancellationToken cancellationToken = default)
    {
        var command = new UpdateQuizCommand(id, quiz);
        var quizId = await _dispatcher.Send(command, cancellationToken);
        
        return Ok(quizId);
    }

    /// <summary>
    /// Changes the status of an existing quiz
    /// </summary>
    /// <param name="id">The ID of the quiz to update</param>
    /// <param name="newStatus">The new status to set</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The new status of the quiz</returns>
    /// <response code="200">Returns the new quiz status</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizStatus>> ChangeQuizStatus(Guid id, [FromBody] QuizStatus newStatus, CancellationToken cancellationToken = default)
    {
        var command = new ChangeQuizStatusCommand
        {
            QuizId = id,
            NewStatus = newStatus
        };
        
        var status = await _dispatcher.Send(command, cancellationToken);
        return Ok(status);
    }

    /// <summary>
    /// Deletes a quiz
    /// </summary>
    /// <param name="id">The ID of the quiz to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the deleted quiz</returns>
    /// <response code="200">Returns the deleted quiz ID</response>
    /// <response code="400">If the quiz cannot be deleted (not in Draft status)</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> DeleteQuiz(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteQuizCommand(id);
        var quizId = await _dispatcher.Send(command, cancellationToken);
        
        return Ok(quizId);
    }
} 