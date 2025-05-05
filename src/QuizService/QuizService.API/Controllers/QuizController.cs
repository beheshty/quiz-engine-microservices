using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Commands.CreateQuiz;
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
} 