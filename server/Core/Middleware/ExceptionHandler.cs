using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Ecommerce.Shared.Model;
using Ecommerce.Shared.Model.Response;

namespace Ecommerce.Core.Middleware;

public class ExceptionHandler : IMiddleware
{
    private readonly ILogger<ExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandler(ILogger<ExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Message: {}", ex.Message);
            _logger.LogError("Details: {}", ex.StackTrace);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _environment.IsDevelopment()
            ? new ApiError(ex.Message, ex.StackTrace?.ToString() ?? string.Empty)
            : new ApiError(context.Response.StatusCode);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}