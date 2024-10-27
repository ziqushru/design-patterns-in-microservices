using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using Core.Domain.Abstractions.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Application.Abstractions.Repositories;

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