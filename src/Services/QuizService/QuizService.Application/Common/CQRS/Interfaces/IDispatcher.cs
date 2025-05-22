using System.Threading;
using System.Threading.Tasks;

namespace QuizService.Application.Common.CQRS.Interfaces;

public interface IDispatcher
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
} 