using System;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Core.Domain.Entities;

namespace Consumers.Core.Application.Abstractions.Services;

public interface IConsumersService
{
    Task<Guid> CreateAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default);
}
