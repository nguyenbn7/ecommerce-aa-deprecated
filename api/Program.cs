using Ecommerce.Core;

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

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseStaticFiles();

app.UseCors("DevCor");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<HandleNotFoundRouteMiddleware>();

app.MapControllers();

await app.SeedAppData();

app.Run();
