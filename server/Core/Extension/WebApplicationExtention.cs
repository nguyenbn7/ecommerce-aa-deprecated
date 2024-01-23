using Ecommerce.Core.Database;
using Ecommerce.Data;
using Ecommerce.Module.Accounts.Model;
using Ecommerce.Shared.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Core.Extension;

public static class WebApplicationExtentions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();

        context = app.Configuration.GetValue<string>("DatabaseProvider") switch
        {
            "Sqlite" => services.GetRequiredService<SqliteAppDbContext>(),
            _ => services.GetRequiredService<PostgreSqlAppDbContext>(),
        };

        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("An error occured during migration with error details below: {}", ex.Message);
            logger.LogError("{}", ex.StackTrace);
        }
    }

    public static async Task SeedAppData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        var context = services.GetRequiredService<AppDbContext>();

        context = app.Configuration.GetValue<string>("DatabaseProvider") switch
        {
            "Sqlite" => services.GetRequiredService<SqliteAppDbContext>(),
            _ => services.GetRequiredService<PostgreSqlAppDbContext>(),
        };

        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            // await DataSeed.CreateRolesAsync(roleManager);
            // await DataSeed.CreateAppAdminAsync(userManager);
            await DataSeed.SeedProductBrandsAsync(context, logger);
            await DataSeed.SeedProductTypesAsync(context, logger);
            await DataSeed.SeedProductsAsync(context, logger);
            await DataSeed.SeedUsersAsync(userManager);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occured during seed data with error details below: {}", ex.Message);
            logger.LogError("{}", ex.StackTrace);
        }
    }
}