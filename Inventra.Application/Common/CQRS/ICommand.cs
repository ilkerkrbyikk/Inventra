using MediatR;
using Inventra.Application.Common.Results;

namespace Inventra.Application.Common.CQRS
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}