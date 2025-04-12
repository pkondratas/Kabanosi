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
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter is not null)
            query = query.Where(filter);

        if (orderBy is not null)
            orderBy(query);

        return await query
            .AsNoTracking()
            .Skip(pageSize * pageNumber)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task Insert(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(object id)
    {
        var entityToDelete = await dbSet.FindAsync(id) ?? throw new NotFoundException(ErrorMessages.ENTITY_NOT_FOUND);
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

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}