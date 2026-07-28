using MediatR;
using Inventra.Application.Common.Results;

namespace Inventra.Application.Common.CQRS
{
    /// <summary>
    /// Base interface for query handlers.
    /// </summary>
    /// <typeparam name="TQuery">Type of query being handled.</typeparam>
    /// <typeparam name="TResponse">Type of data returned by the handler.</typeparam>
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}