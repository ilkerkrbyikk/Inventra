using MediatR;
using Inventra.Application.Common.Results;

namespace Inventra.Application.Common.CQRS
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}