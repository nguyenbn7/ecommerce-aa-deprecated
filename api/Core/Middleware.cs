using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Ecommerce.Share.Model;

namespace Ecommerce.Core.Middleware;

public class HandleNotFoundRouteMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stream originalBody = context.Response.Body;

        try
        {
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await next(context);

            memStream.Position = 0;
            string responseBody = new StreamReader(memStream).ReadToEnd();
            if (string.IsNullOrEmpty(responseBody))
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var response = new ErrorResponse(500, "Unknown error");

                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    response.Message = "Route not found";
                }
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    response.Message = ErrorResponse.GetDefaultMessage(401);
                }

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }

            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }
}

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> logger;
    private readonly IHostEnvironment env;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, IHostEnvironment env)
    {
        this.logger = logger;
        this.env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError("Message: {}", ex.Message);
            logger.LogError("Details: {}", ex.StackTrace);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = env.IsDevelopment()
            ? new ErrorResponse(ex.Message, ex.StackTrace?.ToString())
            : new ErrorResponse(context.Response.StatusCode);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}