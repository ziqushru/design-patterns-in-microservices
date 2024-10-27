using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Application.Abstractions.Services;
using Contracts.Core.Domain.Entities;

namespace Contracts.Core.Application.Services;

public class ContractsService(
    IContractsRepository contractsRepository)
    : IContractsService
{
    public async Task<Guid> CreateAsync(
        Contract contract,
        CancellationToken cancellationToken = default)
    {
        var id = await contractsRepository.CreateAsync(contract, cancellationToken);

        return id;
    }

    public async Task DeleteAsync(
        Contract contract,
        CancellationToken cancellationToken = default)
    {
        await contractsRepository.DeleteAsync(contract, cancellationToken);
    }

    public async Task UpdateAsync(
        Contract contract,
        CancellationToken cancellationToken = default)
    {
        await contractsRepository.UpdateAsync(contract, cancellationToken);
    }
}
