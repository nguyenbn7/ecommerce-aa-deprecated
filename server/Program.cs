using Ecommerce.Core.Extension;
using Ecommerce.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAppDbContext(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddAuthentication();

builder.Services.AddAuthorization();

builder.Services.UseIdentity(builder.Configuration, builder.Environment);

builder.Services.UseSwagger();

builder.Services.AddAppServices();

builder.Services.ConfigureApiBehaviourOptions();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandler>();

app.UseStaticFiles();

app.UseCors("DevCor");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<RouteNotFoundHandler>();

app.MapControllers();

await app.ApplyMigrations();

await app.SeedAppData();

app.Run();
