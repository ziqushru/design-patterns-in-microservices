using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Abstractions.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Services;

public class OrdersService(
    IOrdersRepository ordersRepository)
    : IOrdersService
{
    public async Task<Guid> CreateAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        order.Price = order.OrderItems.Sum(x => x.Price * x.Quantity);
        order.OrderNumber = await ordersRepository.GetNextOrderNumberAsync(cancellationToken);
        order.DateSubmitted = DateTime.UtcNow;

        var id = await ordersRepository.CreateAsync(order, cancellationToken);

        return id;
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var order = await ordersRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Δεν βρέθηκε η παραγγελία");

        await ordersRepository.DeleteAsync(order, cancellationToken);
    }

    public async Task UpdateAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        await ordersRepository.UpdateAsync(order, cancellationToken);
    }
}
