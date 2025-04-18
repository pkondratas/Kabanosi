using System.Data;
using System.Linq.Expressions;
using Kabanosi.Constants;
using Kabanosi.Exceptions;
using Kabanosi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Repositories;

public abstract class GenericRepository<TEntity>(DatabaseContext context)
    where TEntity : class
{
    internal DatabaseContext context = context;
    internal DbSet<TEntity> dbSet = context.Set<TEntity>();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter is not null)
            query = query.Where(filter);

        if (orderBy is not null)
            orderBy(query);

        query = ApplyIncludes(query, includes);

        return await query
            .AsNoTracking()
            .Skip(pageSize * pageNumber)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await dbSet.FindAsync([id], cancellationToken);
    }
    
    public virtual async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return (await dbSet
                .AddAsync(entity, cancellationToken))
            .Entity;
    }

    public virtual async Task DeleteAsync(object id, CancellationToken cancellationToken)
    {
        var entityToDelete = await dbSet.FindAsync([id], cancellationToken) ??
                             throw new NotFoundException(ErrorMessages.ENTITY_NOT_FOUND);
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }

        dbSet.Remove(entityToDelete);
    }

    private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[] includes)
    {
        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }
}