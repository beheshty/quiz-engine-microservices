using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Domain.Entities.QuizRuntime;

namespace QuizService.Infrastructure.Data.Configurations;

public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
{
    public void Configure(EntityTypeBuilder<UserAnswer> builder)
    {
        builder.ToTable("UserAnswers");

        builder.HasKey(ua => ua.Id);

        builder.Property(ua => ua.AnswerText)
            .IsRequired()
            .HasMaxLength(1000);
    }
} 