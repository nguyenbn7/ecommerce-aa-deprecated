using System.Text;
using Ecommerce.API.Accounts;
using Ecommerce.API.Baskets;
using Ecommerce.Core.Database;
using Ecommerce.Core.Middleware;
using Ecommerce.Share.GenericRepository;
using Ecommerce.Share.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Ecommerce.Core.Extensions;

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

    public static IServiceCollection AddStoreContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DevelopmentConn"));
        });
        services.AddScoped<DbContext, StoreContext>();
        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authentication Bearer Scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securitySchema);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    securitySchema, new [] {"Bearer"}
                }
            };

            c.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DevelopmentConn"));
        });

        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            // add identity options here
        })
        .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
        .AddSignInManager<SignInManager<ApplicationUser>>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var appKey = config["Token:Key"];
                var appIssuer = config["Token:Issuer"];
                if (appKey == null)
                {
                    throw new Exception("Token:Key is null");
                }

                if (appIssuer == null)
                {
                    throw new Exception("Token:Issuer is null");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appKey)),
                    ValidIssuer = appIssuer,
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization();

        return services;
    }
}