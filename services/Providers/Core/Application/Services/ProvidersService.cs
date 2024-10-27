using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Providers.Core.Application.Abstractions.Repositories;
using Providers.Core.Application.Abstractions.Services;
using Providers.Core.Domain.Entities;
using static Messaging.Contracts.ProviderDomainEvents;

namespace Providers.Core.Application.Services;

public class ProvidersService(
    IPublishEndpoint publishEndpoint,
    IProvidersRepository providersRepository)
    : IProvidersService
{
    public async Task<Guid> CreateAsync(
        Provider provider,
        CancellationToken cancellationToken = default)
    {
        var id = await providersRepository.CreateAsync(provider, cancellationToken);

        await publishEndpoint.Publish(new Created
        {
            Id = id,
            BrandName = provider.BrandName
        },
        cancellationToken);

        return id;
    }

    public async Task DeleteAsync(
        Provider provider,
        CancellationToken cancellationToken = default)
    {
        await providersRepository.DeleteAsync(provider, cancellationToken);

        await publishEndpoint.Publish(new Deleted
        {
            Id = provider.Id
        },
        cancellationToken);
    }

    public async Task UpdateAsync(
        Provider provider,
        CancellationToken cancellationToken = default)
    {
        await providersRepository.UpdateAsync(provider, cancellationToken);

        await publishEndpoint.Publish(new Updated
        {
            Id = provider.Id,
            BrandName = provider.BrandName
        },
        cancellationToken);
    }
}
