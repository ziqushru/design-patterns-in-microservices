using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Abstractions.Repositories;

public interface IOrdersRepository : IBaseRepository<Order>
{
    Task<int> GetNextOrderNumberAsync(
        CancellationToken cancellationToken = default);
}
