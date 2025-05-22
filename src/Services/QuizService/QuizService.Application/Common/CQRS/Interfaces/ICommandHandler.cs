using System.Threading;
using System.Threading.Tasks;

namespace QuizService.Application.Common.CQRS.Interfaces;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
} 