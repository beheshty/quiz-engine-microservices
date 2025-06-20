using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Application.Commands.ProcessUserQuiz;
using QuizService.Application.Commands.DraftUserQuiz;
using QuizService.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace QuizService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserQuizController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public UserQuizController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Processes the user's answers for a quiz.
    /// </summary>
    /// <param name="request">The request containing user quiz ID, user ID, and answers.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of processing the user quiz, including the updated status.</returns>
    /// <response code="200">Returns the result of processing the user quiz.</response>
    /// <response code="400">If the request data is invalid or the quiz is already completed.</response>
    /// <response code="404">If the user quiz is not found for the specified user.</response>
    [HttpPost("process")]
    [ProducesResponseType(typeof(ProcessUserQuizResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProcessUserQuizResultDto>> ProcessUserQuiz(
        [FromBody] ProcessUserQuizRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new ProcessUserQuizCommand(
            request.UserQuizId,
            request.UserId,
            request.Answers.Select(a => new UserQuizAnswerDto
            {
                QuizQuestionId = a.QuizQuestionId,
                AnswerText = a.AnswerText
            }).ToList()
        );

        var result = await _dispatcher.Send(command, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Drafts (starts) a new user quiz instance.
    /// </summary>
    /// <param name="request">The request containing the user ID and quiz ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The details of the newly drafted user quiz.</returns>
    /// <response code="200">Returns the drafted user quiz details.</response>
    /// <response code="400">If the request data is invalid or the quiz is not published.</response>
    /// <response code="404">If the quiz is not found.</response>
    [HttpPost("draft")]
    [ProducesResponseType(typeof(DraftUserQuizResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DraftUserQuizResultDto>> DraftUserQuiz(
        [FromBody] DraftUserQuizRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new DraftUserQuizCommand(request.UserId, request.QuizId);
        var result = await _dispatcher.Send(command, cancellationToken);
        return Ok(result);
    }
} 