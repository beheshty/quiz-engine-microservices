using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;

namespace QuizService.Application.Commands.ChangeQuizStatus
{
    public class ChangeQuizStatusCommandHandler(IQuizRepository quizRepository) : ICommandHandler<ChangeQuizStatusCommand, QuizStatus>
    {
        public async Task<QuizStatus> Handle(ChangeQuizStatusCommand command, CancellationToken cancellationToken = default)
        {
            var quiz = await quizRepository.GetAsync(command.QuizId, cancellationToken);
            quiz.ChangeStatus(command.NewStatus);
            await quizRepository.UpdateAsync(quiz, true, cancellationToken);
            return command.NewStatus;
        }
    }
}
