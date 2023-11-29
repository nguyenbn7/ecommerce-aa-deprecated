using System.Text.Json;
using Ecommerce.Routes.Accounts;
using Ecommerce.Routes.ProductBrands;
using Ecommerce.Routes.Products;
using Ecommerce.Routes.ProductTypes;
using Ecommerce.Core.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Migrations.Seeding;

public class SeedData
{
    public static async Task SeedProductBrandAsync(StoreContext storeContext, ILogger logger)
    {
        if (await storeContext.ProductBrands.AnyAsync()) return;

        var brandsData = await File.ReadAllTextAsync("Migrations/Seeding/brands.json");
        var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
        if (productBrands == null)
        {
            logger.LogError("Can not get seed data: Product Brand");
            return;
        }

        storeContext.ProductBrands.AddRange(productBrands);
        await storeContext.SaveChangesAsync();
    }

    public static async Task SeedProductTypeAsync(StoreContext storeContext, ILogger logger)
    {
        if (await storeContext.ProductTypes.AnyAsync()) return;

        var typesData = await File.ReadAllTextAsync("Migrations/Seeding/types.json");
        var productTypes = JsonSerializer.Deserialize<List<ProductType>>(typesData);
        if (productTypes == null)
        {
            logger.LogError("Can not get seed data: Product Type");
            return;
        }

        storeContext.ProductTypes.AddRange(productTypes);
        await storeContext.SaveChangesAsync();
    }

    public static async Task SeedProductAsync(StoreContext storeContext, ILogger logger)
    {
        if (await storeContext.Products.AnyAsync()) return;

        await SeedProductBrandAsync(storeContext, logger);
        await SeedProductTypeAsync(storeContext, logger);

        var productsData = await File.ReadAllTextAsync("Migrations/Seeding/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
        if (products == null)
        {
            logger.LogError("Can not get seed data: Product");
            return;
        }

        storeContext.Products.AddRange(products);
        await storeContext.SaveChangesAsync();
    }

    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        if (userManager.Users.Any())
            return;

        var user = new ApplicationUser
        {
            DisplayName = "Bob",
            Email = "bob@test.com",
            UserName = "bob@test.com",
            Address = new Address
            {
                FirstName = "Bob",
                LastName = "Bobbity",
                Street = "10 the Street",
                City = "New York",
                State = "NY",
                ZipCode = "90210",
            }
        };

        await userManager.CreateAsync(user, "Pa$$w0rd");
    }
}