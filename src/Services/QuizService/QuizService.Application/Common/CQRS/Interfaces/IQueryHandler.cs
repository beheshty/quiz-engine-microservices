using System.Threading;
using System.Threading.Tasks;

namespace QuizService.Application.Common.CQRS.Interfaces;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
} 