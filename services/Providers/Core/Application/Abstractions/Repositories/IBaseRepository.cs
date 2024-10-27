using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Providers.Core.Domain.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Providers.Core.Application.Abstractions.Repositories;

public interface IBaseRepository<T> where T : Entity
{
    DbSet<T> Table { get; }

    Task<Guid> CreateAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    IQueryable<T> GetByIds(
        IList<Guid?> ids);

    IQueryable<T> GetAll();

    Task<TProjected?> GetByProjectedAsync<TProjected>(
        Expression<Func<T, bool>>? predicate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TProjected>> GetAllByProjectedAsync<TProjected>(
        Expression<Func<T, bool>>? predicate,
        Expression<Func<TProjected, object>>? orderBy,
        bool ascending = true,
        CancellationToken cancellationToken = default);

    Task<Guid> UpdateWithoutDomainEventAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task<Guid> UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task<Guid> DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default);
}
