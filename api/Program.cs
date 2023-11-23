using Ecommerce.Core.Extensions;
using Ecommerce.Core.Middleware;
using Ecommerce.Share.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCustomServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddStoreContext(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value?.Errors ?? new())
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                var errorResponse = new ValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("DevCor", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseStaticFiles();

app.UseCors("DevCor");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<HandleNotFoundRouteMiddleware>();

app.MapControllers();

// await app.SeedAppData();

app.Run();
