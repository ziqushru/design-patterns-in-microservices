using MediatR;

namespace Providers.Core.Application.Abstractions.Queries;

public interface IAppQuery<out TResponse> : IRequest<TResponse>;
