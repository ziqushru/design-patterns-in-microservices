using MediatR;

namespace Contracts.Core.Application.Abstractions.Queries;

public interface IAppQuery<out TResponse> : IRequest<TResponse>;
