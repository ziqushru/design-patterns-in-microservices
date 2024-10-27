using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Core.Domain.Entities;

namespace Contracts.Core.Application.Abstractions.Services;

public interface IContractsService
{
    Task<Guid> CreateAsync(
        Contract contract,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Contract contract,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Contract contract,
        CancellationToken cancellationToken = default);
}
