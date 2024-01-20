using Ecommerce.Module.Accounts;
using Ecommerce.Module.ProductBrands;
using Ecommerce.Module.Products;
using Ecommerce.Module.ProductTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Shared;

public abstract class AppDbContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    protected IConfiguration Configuration { get; }

    protected AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public required DbSet<Product> Products { get; set; }
    public required DbSet<ProductBrand> ProductBrands { get; set; }
    public required DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<IdentityUserClaim<string>>();
        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityRoleClaim<string>>();
        builder.Ignore<IdentityUserToken<string>>();

        builder.Entity<AppUser>()
            .HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.Entity<AppUserRole>().ToTable("UserRoles");
        builder.Entity<AppRole>().ToTable("Roles");
        builder.Entity<AppUser>().ToTable("Users");
    }
}

public class SqliteAppDbContext : AppDbContext
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
    }
}

public class PostgreSqlAppDbContext : AppDbContext
{
    public PostgreSqlAppDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostgreConn"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}