using Ecommerce.API.Basket.Repository;
using Ecommerce.Share;
using StackExchange.Redis;

namespace Ecommerce.Core;

public static class ServicesExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddTransient<ExceptionHandlerMiddleware>();
        services.AddTransient<HandleNotFoundRouteMiddleware>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            // TODO: get url in environment
            var connectionString = configuration.GetConnectionString("Redis");

            if (string.IsNullOrEmpty(connectionString?.Trim()))
                throw new Exception("Can not find redis connection string");

            var options = ConfigurationOptions.Parse(connectionString);
            return ConnectionMultiplexer.Connect(options);
        });

        return services;
    }
}