using MediatR;

namespace Consumers.Core.Application.Abstractions.Queries;

public interface IAppQuery<out TResponse> : IRequest<TResponse>;
