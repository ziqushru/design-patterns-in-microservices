using MediatR;

namespace Core.Application.Abstractions.Queries;

public interface IAppQuery<out TResponse> : IRequest<TResponse>;
