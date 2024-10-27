using MediatR;

namespace Core.Application.Abstractions.Commands;

public interface IAppCommandHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IAppCommand<TResponse>;
