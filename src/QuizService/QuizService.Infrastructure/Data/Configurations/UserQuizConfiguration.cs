using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Domain.Entities.QuizRuntime;
using QuizService.Domain.Enums;

namespace QuizService.Infrastructure.Data.Configurations;

public class UserQuizConfiguration : IEntityTypeConfiguration<UserQuiz>
{
    public void Configure(EntityTypeBuilder<UserQuiz> builder)
    {
        builder.ToTable("UserQuizzes");

        builder.HasKey(uq => uq.Id);

        builder.HasMany(uq => uq.Answers)
            .WithOne()
            .HasForeignKey(ua => ua.UserQuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 