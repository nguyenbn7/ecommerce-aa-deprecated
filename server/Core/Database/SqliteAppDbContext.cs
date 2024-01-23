using System.Reflection;
using Ecommerce.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Database;

public sealed class SqliteAppDbContext : AppDbContext
{
    public SqliteAppDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("SqliteConn"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}