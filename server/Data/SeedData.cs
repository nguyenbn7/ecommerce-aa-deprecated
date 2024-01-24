using System.Text.Json;
using Ecommerce.Module.Accounts.Model;
using Ecommerce.Module.Orders.Model;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public class DataSeed
{
    public static async Task SeedProductBrandsAsync(AppDbContext context,
                                                    ILogger logger)
    {
        if (await context.ProductBrands.AnyAsync()) return;

        var brandsData = await File.ReadAllTextAsync("Data/brands.json");
        var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
        if (productBrands == null)
        {
            logger.LogError("Can not get seed data: Product Brand");
            return;
        }

        context.ProductBrands.AddRange(productBrands);
        await context.SaveChangesAsync();
    }

    public static async Task SeedProductTypesAsync(AppDbContext context,
                                                   ILogger logger)
    {
        if (await context.ProductTypes.AnyAsync()) return;

        var typesData = await File.ReadAllTextAsync("Data/types.json");
        var productTypes = JsonSerializer.Deserialize<List<ProductType>>(typesData);
        if (productTypes == null)
        {
            logger.LogError("Can not get seed data: Product Type");
            return;
        }

        context.ProductTypes.AddRange(productTypes);
        await context.SaveChangesAsync();
    }

    public static async Task SeedProductsAsync(AppDbContext context,
                                               ILogger logger)
    {
        if (await context.Products.AnyAsync()) return;

        var productsData = await File.ReadAllTextAsync("Data/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
        if (products == null)
        {
            logger.LogError("Can not get seed data: Product");
            return;
        }

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }

    public static async Task SeedDeliveryMethodsAsync(AppDbContext context,
                                               ILogger logger)
    {
        if (await context.DeliveryMethods.AnyAsync()) return;

        var deliveryMethodsData = await File.ReadAllTextAsync("Data/delivery.json");
        var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
        if (deliveryMethods == null)
        {
            logger.LogError("Can not get seed data: Delivery Method");
            return;
        }

        context.DeliveryMethods.AddRange(deliveryMethods);
        await context.SaveChangesAsync();
    }

    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (userManager.Users.Any())
            return;

        var user = new AppUser
        {
            DisplayName = "Bob",
            Email = "bob@test.com",
            UserName = "bob@test.com",
            Address = new Module.Accounts.Model.Address
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