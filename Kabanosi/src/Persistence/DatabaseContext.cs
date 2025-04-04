using Microsoft.EntityFrameworkCore;

namespace Kabanosi.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}