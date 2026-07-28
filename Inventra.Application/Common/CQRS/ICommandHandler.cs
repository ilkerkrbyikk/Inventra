using MediatR;
using Inventra.Application.Common.Results;

namespace Inventra.Application.Common.CQRS
{
    /// <summary>
    /// Base interface for command handlers that return no data.
    /// </summary>
    /// <typeparam name="TCommand">Type of command being handled.</typeparam>
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }

    /// <summary>
    /// Base interface for command handlers that return data.
    /// </summary>
    /// <typeparam name="TCommand">Type of command being handled.</typeparam>
    /// <typeparam name="TResponse">Type of data returned by the handler.</typeparam>
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}