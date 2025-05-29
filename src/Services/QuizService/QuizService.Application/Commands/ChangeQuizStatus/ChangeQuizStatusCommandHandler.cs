using QuizService.Application.Common.CQRS.Interfaces;
using QuizService.Domain.Enums;
using QuizService.Domain.Repositories;

namespace QuizService.Application.Commands.ChangeQuizStatus
{
    public class ChangeQuizStatusCommandHandler : ICommandHandler<ChangeQuizStatusCommand, QuizStatus>
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeQuizStatusCommandHandler(IQuizRepository quizRepository, IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<QuizStatus> Handle(ChangeQuizStatusCommand command, CancellationToken cancellationToken = default)
        {
            var quiz = await _quizRepository.GetAsync(command.QuizId, cancellationToken);
            quiz.ChangeStatus(command.NewStatus);
            await _quizRepository.UpdateAsync(quiz, cancellationToken: cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return command.NewStatus;
        }
    }
}
