using System;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Core.Application.Abstractions.Repositories;
using Consumers.Core.Application.Abstractions.Services;
using Consumers.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Consumers.Core.Application.Services;

public class ConsumersService(
    IPublishEndpoint publishEndpoint,
    IConsumersRepository consumersRepository)
    : IConsumersService
{
    public async Task<Guid> CreateAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default)
    {
        var id = await consumersRepository.CreateAsync(consumer, cancellationToken);

        await publishEndpoint.Publish(new Created
        {
            Id = id,
            FirstName = consumer.FirstName,
            LastName = consumer.LastName,
            Type = (int)consumer.Type
        },
        cancellationToken);

        return id;
    }

    public async Task DeleteAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default)
    {
        await consumersRepository.DeleteAsync(consumer, cancellationToken);

        await publishEndpoint.Publish(new Deleted
        {
            Id = consumer.Id
        },
        cancellationToken);
    }

    public async Task UpdateAsync(
        Consumer consumer,
        CancellationToken cancellationToken = default)
    {
        await consumersRepository.UpdateAsync(consumer, cancellationToken);

        await publishEndpoint.Publish(new Updated
        {
            Id = consumer.Id,
            FirstName = consumer.FirstName,
            LastName = consumer.LastName,
            Type = (int)consumer.Type
        },
        cancellationToken);
    }
}
