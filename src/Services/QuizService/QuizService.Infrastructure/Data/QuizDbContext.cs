using Microsoft.EntityFrameworkCore;
using QuizService.Domain.Entities.QuizManagement;
using QuizService.Domain.Entities.QuizRuntime;

namespace QuizService.Infrastructure.Data;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
    {
    }

    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; } 
    public DbSet<UserQuiz> UserQuizzes { get; set; } 
    public DbSet<UserAnswer> UserAnswers { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizDbContext).Assembly);
    }
} 