using System.Text;
using Ecommerce.Module.Baskets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Ecommerce.Shared.Service;
using Ecommerce.Core.Middleware;
using Ecommerce.Shared.Model;
using Ecommerce.Core.Database;
using Ecommerce.Shared.Database;
using Ecommerce.Module.Accounts.Model;
using Ecommerce.Shared.Model.Response;

namespace Ecommerce.Core.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return configuration.GetValue<string>("DatabaseProvider") switch
        {
            "Sqlite" => services.AddDbContext<AppDbContext, SqliteAppDbContext>(),
            _ => services.AddDbContext<AppDbContext, PostgreSqlAppDbContext>()
        };
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(Repository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<AuthenticationService, JWTAuthenticationService>();

        services.AddTransient<ExceptionHandler>();
        services.AddTransient<RouteNotFoundHandler>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            // TODO: get url in environment
            var connectionString = configuration.GetConnectionString("RedisConn");

            if (string.IsNullOrEmpty(connectionString?.Trim()))
                throw new Exception("Can not find redis connection string");

            var options = ConfigurationOptions.Parse(connectionString);
            return ConnectionMultiplexer.Connect(options);
        });

        return services;
    }

    public static IServiceCollection UseSwagger(this IServiceCollection services)
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

    public static IServiceCollection UseIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddAuthentication();

        services.AddIdentityCore<AppUser>(options =>
        {
            // Add options here
        })
            .AddRoles<AppRole>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization();

        services.Configure<IdentityOptions>(options =>
        {
            // Configure more if needed
            var passwordSettingsConfig = configuration.GetSection("Identity:Options:Password");

            // Password settings.
            options.Password.RequireDigit = !env.IsDevelopment();
            options.Password.RequireLowercase = !env.IsDevelopment();
            options.Password.RequireNonAlphanumeric = !env.IsDevelopment();
            options.Password.RequireUppercase = !env.IsDevelopment();
            options.Password.RequiredLength = passwordSettingsConfig.GetValue<int>("RequiredLength");
            options.Password.RequiredUniqueChars = passwordSettingsConfig.GetValue<int>("RequiredUniqueChars");

            var lockoutSettingsConfig = configuration.GetSection("Identity:Options:Lockout");

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutSettingsConfig.GetValue<double>("DefaultLockoutTimeSpanMinutes"));
            options.Lockout.MaxFailedAccessAttempts = lockoutSettingsConfig.GetValue<int>("MaxFailedAccessAttempts");
            options.Lockout.AllowedForNewUsers = lockoutSettingsConfig.GetValue<bool>("AllowedForNewUsers");

            var userSettingsConfig = configuration.GetSection("Identity:Options:User");

            // User settings.
            options.User.AllowedUserNameCharacters = userSettingsConfig.GetValue<string>("AllowedUserNameCharacters") ?? options.User.AllowedUserNameCharacters;
            options.User.RequireUniqueEmail = userSettingsConfig.GetValue<bool>("RequireUniqueEmail");
        });

        return services;
    }

    public static IServiceCollection UseJWTForAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSettingsConfig = configuration.GetSection("Token");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var appKey = tokenSettingsConfig["Key"];
                var appIssuer = tokenSettingsConfig["Issuer"];
                if (appKey == null)
                {
                    throw new Exception("Token Key not found");
                }

                if (appIssuer == null)
                {
                    throw new Exception("Token Issuer not found");
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

        return services;
    }

    public static IServiceCollection ConfigureApiBehaviourOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value?.Errors ?? new())
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                var errorResponse = new ValidationError(400, errors);

                return new BadRequestObjectResult(errorResponse);
            };
        });
        return services;
    }
}