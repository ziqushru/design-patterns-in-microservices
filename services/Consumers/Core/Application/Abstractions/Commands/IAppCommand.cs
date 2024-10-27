using MediatR;

namespace Core.Application.Abstractions.Commands;

public interface IAppCommand<out TResponse> : IRequest<TResponse>;
