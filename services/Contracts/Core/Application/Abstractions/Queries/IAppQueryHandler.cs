using MediatR;

namespace Contracts.Core.Application.Abstractions.Queries;

public interface IAppQueryHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse?>
    where TRequest : IAppQuery<TResponse?>;

