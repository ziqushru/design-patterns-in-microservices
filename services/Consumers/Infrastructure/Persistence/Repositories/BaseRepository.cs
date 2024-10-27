using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Application.Abstractions.Repositories;
using Core.Domain.Abstractions.Entities;
using Core.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T>(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : IBaseRepository<T>
    where T : Entity
{
    public DbSet<T> Table => applicationContext.Set<T>();

    public async Task<Guid> CreateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Table.Add(entity);

        await applicationContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    =>
        await Table.AnyAsync(entity =>
            entity.Id == id,
            cancellationToken);

    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    =>
        await Table.FindAsync([id],
        cancellationToken);

    public IQueryable<T> GetByIds(
        IList<Guid?> ids)
    =>
        Table.Where(entity => ids.Contains(entity.Id));

    public IQueryable<T> GetAll() =>
        Table;

    public async Task<TProjected?> GetByProjectedAsync<TProjected>(
        Expression<Func<T, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        var query = Table.AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query
            .ProjectTo<TProjected>(mapperConfiguration)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TProjected>> GetAllByProjectedAsync<TProjected>(
        Expression<Func<T, bool>>? predicate,
        Expression<Func<TProjected, object>>? orderBy,
        bool ascending = true,
        CancellationToken cancellationToken = default)
    {
        var query = Table.AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        var projectedQuery = query.ProjectTo<TProjected>(mapperConfiguration);

        if (orderBy != null)
        {
            projectedQuery = ascending ?
                projectedQuery.OrderBy(orderBy) :
                projectedQuery.OrderByDescending(orderBy);
        }

        return await projectedQuery.ToListAsync(cancellationToken);
    }

    public async Task<Guid> UpdateWithoutDomainEventAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Table.Update(entity);

        await applicationContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<Guid> UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Table.Update(entity);

        await applicationContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<Guid> DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        if (await HasReferencesAsync(entity, cancellationToken))
        {
            throw new ReferencedDataDeleteException(typeof(T).Name);
        }

        Table.Remove(entity);

        await applicationContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    private async Task<bool> HasReferencesAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        var entityType = typeof(T);
        var entityPrimaryKey = applicationContext.Model.FindEntityType(entityType)?.FindPrimaryKey()?.Properties[0];
        var primaryKeyValue = entityPrimaryKey?.PropertyInfo?.GetValue(entity);

        var referencingForeignKeys = applicationContext.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .Where(fk => fk.PrincipalEntityType.ClrType == entityType && fk.DeleteBehavior == DeleteBehavior.Restrict)
            .ToList();

        foreach (var foreignKey in referencingForeignKeys)
        {
            var referencingEntityType = foreignKey.DeclaringEntityType.ClrType;
            var referencingDbSet = GetDbSet(referencingEntityType);
            var foreignKeyProperty = foreignKey.Properties[0].PropertyInfo!;

            var parameter = Expression.Parameter(referencingEntityType, "e");
            var propertyAccess = Expression.MakeMemberAccess(parameter, foreignKeyProperty);
            var constant = Expression.Constant(primaryKeyValue);
            var equalsExpression = Expression.Equal(propertyAccess, constant);
            var lambda = Expression.Lambda(equalsExpression, parameter);

            var queryable = referencingDbSet.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    "Where",
                    [referencingEntityType],
                    referencingDbSet.Expression,
                    lambda));

            var anyAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == nameof(EntityFrameworkQueryableExtensions.AnyAsync) &&
                    m.GetParameters().Length == 2 &&
                    m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>))
                ?? throw new ReferencedDataDeleteException("AnyAsync method not found.");

            var genericAnyAsyncMethod = anyAsyncMethod.MakeGenericMethod(referencingEntityType);

            var anyAsyncTask = (Task<bool>)genericAnyAsyncMethod.Invoke(null, [queryable, cancellationToken])!;

            if (await anyAsyncTask)
            {
                return true;
            }
        }

        return false;
    }

    private IQueryable<object> GetDbSet(
        Type entityType)
    {
        var method = typeof(DbContext)
            .GetMethods()
            .First(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0)
            .MakeGenericMethod(entityType);

        return (IQueryable<object>)method.Invoke(applicationContext, null)!;
    }
}
