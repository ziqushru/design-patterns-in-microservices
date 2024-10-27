using MediatR;

namespace Providers.Core.Application.Abstractions.Commands;

public interface IAppCommandHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IAppCommand<TResponse>;

public interface IAppCommandHandlerWithoutResponse<in TRequest>
    : IRequestHandler<TRequest>
    where TRequest : IAppCommandWithoutResponse;
