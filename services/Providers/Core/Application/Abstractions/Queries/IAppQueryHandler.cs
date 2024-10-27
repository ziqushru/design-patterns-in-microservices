using MediatR;

namespace Providers.Core.Application.Abstractions.Queries;

public interface IAppQueryHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse?>
    where TRequest : IAppQuery<TResponse?>;

