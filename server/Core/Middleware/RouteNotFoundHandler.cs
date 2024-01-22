using System.Net.Mime;
using System.Text.Json;
using Ecommerce.Shared.Model;

namespace Ecommerce.Core.Middleware;

public class RouteNotFoundHandler : IMiddleware
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