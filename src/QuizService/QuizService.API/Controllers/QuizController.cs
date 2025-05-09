using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Commands.CreateQuiz;
using QuizService.Application.Commands.UpdateQuiz;
using QuizService.Application.Common.CQRS.Interfaces;

namespace QuizService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public QuizController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
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
    /// <response code="404">If the quiz is not found</response>
    /// <response code="409">If the quiz is not in Draft status</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Guid>> UpdateQuiz(Guid id, UpdateQuizDto quiz, CancellationToken cancellationToken = default)
    {
        var command = new UpdateQuizCommand(id, quiz);
        var quizId = await _dispatcher.Send(command, cancellationToken);
        
        return Ok(quizId);
    }
} 