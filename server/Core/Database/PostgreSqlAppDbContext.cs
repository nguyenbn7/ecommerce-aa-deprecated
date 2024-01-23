using Ecommerce.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Database;

public sealed class PostgreSqlAppDbContext : AppDbContext
{
    public PostgreSqlAppDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostgreConn"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}