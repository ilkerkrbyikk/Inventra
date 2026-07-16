using MediatR;
using Inventra.Application.Common.Results;

namespace Inventra.Application.Common.CQRS
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}