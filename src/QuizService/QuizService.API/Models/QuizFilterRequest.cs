using System.ComponentModel.DataAnnotations;
using QuizService.Domain.Enums;

namespace QuizService.API.Models;

/// <summary>
/// Request model for filtering quizzes
/// </summary>
public class QuizFilterRequest
{
    /// <summary>
    /// Filter by quiz title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Filter by quiz status
    /// </summary>
    public QuizStatus? Status { get; set; }

    /// <summary>
    /// Filter by creation date (from)
    /// </summary>
    public DateTime? CreatedFrom { get; set; }

    /// <summary>
    /// Filter by creation date (to)
    /// </summary>
    public DateTime? CreatedTo { get; set; }
} 