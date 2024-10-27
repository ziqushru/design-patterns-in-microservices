using System;
using System.Threading;
using System.Threading.Tasks;
using Providers.Core.Application.Abstractions.Repositories;
using Providers.Core.Application.Abstractions.Services;
using Providers.Core.Domain.Entities;

namespace Providers.Core.Application.Services;

public class ProvidersService(
    IProvidersRepository providersRepository)
    : IProvidersService
{
    public async Task<Guid> CreateAsync(
        Provider provider,
        CancellationToken cancellationToken = default)
    {
        var id = await providersRepository.CreateAsync(provider, cancellationToken);

        return id;
    }

    public async Task DeleteAsync(
        Provider provider,
        CancellationToken cancellationToken = default)
    {
        await providersRepository.DeleteAsync(provider, cancellationToken);
    }

    public async Task UpdateAsync(
        Provider provider,
        CancellationToken cancellationToken = default)
    {
        await providersRepository.UpdateAsync(provider, cancellationToken);
    }
}
