using Kabanosi.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kabanosi.Repositories.UnitOfWork
{
    public class UnitOfWork(DatabaseContext context) : IUnitOfWork, IAsyncDisposable
    {
        private bool _disposed;
        private IDbContextTransaction _transaction = null!;
        public DatabaseContext Context { get; } = context;

        public async Task CreateTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
        
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
        
        private async Task DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    await Context.DisposeAsync();

                _disposed = true;
            }
        }
    }      
}