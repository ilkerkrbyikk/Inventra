using MediatR;
using Inventra.Application.Common.Results;

namespace Inventra.Application.Common.CQRS
{
    /// <summary>
    /// Base interface for commands that return no data (only success/failure).
    /// </summary>
    public interface ICommand : IRequest<Result>
    {
    }

    /// <summary>
    /// Base interface for commands that return data.
    /// </summary>
    /// <typeparam name="TResponse">Type of data returned by the command.</typeparam>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}