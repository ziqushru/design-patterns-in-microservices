using System;
using System.Threading;
using System.Threading.Tasks;
using Providers.Core.Domain.Entities;

namespace Providers.Core.Application.Abstractions.Services;

public interface IProvidersService
{
    Task<Guid> CreateAsync(
        Provider provider,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Provider provider,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Provider provider,
        CancellationToken cancellationToken = default);
}
