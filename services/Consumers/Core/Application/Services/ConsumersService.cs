using System;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Core.Application.Abstractions.Repositories;
using Consumers.Core.Application.Abstractions.Services;
using Consumers.Core.Domain.Entities;

namespace Consumers.Core.Application.Services;

public class ConsumersService(
    IConsumersRepository consumersRepository)
    : IConsumersService
{
    public async Task<Guid> CreateAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default)
    {
        var id = await consumersRepository.CreateAsync(consumer, cancellationToken);

        return id;
    }

    public async Task DeleteAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default)
    {
        await consumersRepository.DeleteAsync(consumer, cancellationToken);
    }

    public async Task UpdateAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default)
    {
        await consumersRepository.UpdateAsync(consumer, cancellationToken);
    }
}
