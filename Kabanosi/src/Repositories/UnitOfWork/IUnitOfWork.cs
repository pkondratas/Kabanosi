using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Repositories.UnitOfWork;

public interface IUnitOfWork<out TContext> 
    where TContext : DbContext
{
    public TContext Context { get; }

    Task CreateTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveAsync();
}