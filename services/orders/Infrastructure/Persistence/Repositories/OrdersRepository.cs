using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrdersRepository(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : BaseRepository<Order>(applicationContext, mapperConfiguration), IOrdersRepository
{
    public async Task<int> GetNextOrderNumberAsync(
        CancellationToken cancellationToken = default)
    {
        var maxOrderNumber = 0;

        if (await Table.AnyAsync(cancellationToken))
        {
            maxOrderNumber = await Table.MaxAsync(order =>
                order.OrderNumber, cancellationToken);
        }

        return maxOrderNumber + 1;
    }
}
