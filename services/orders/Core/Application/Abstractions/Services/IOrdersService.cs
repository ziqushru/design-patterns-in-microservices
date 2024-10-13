using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Abstractions.Services;

public interface IOrdersService
{
    Task<Guid> CreateAsync(
        Order order,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Order order,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
