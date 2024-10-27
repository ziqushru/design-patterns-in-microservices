using MediatR;

namespace Contracts.Core.Application.Abstractions.Commands;

public interface IAppCommand<out TResponse> : IRequest<TResponse>;

public interface IAppCommandWithoutResponse : IRequest;
