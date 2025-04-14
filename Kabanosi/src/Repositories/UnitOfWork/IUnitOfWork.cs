using Kabanosi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    public DatabaseContext Context { get; }

    Task CreateTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveAsync();
}