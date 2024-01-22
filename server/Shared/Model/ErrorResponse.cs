using System.Text.Json.Serialization;

namespace Ecommerce.Shared.Model;

public class ErrorResponse
{
    [JsonIgnore]
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Details { get; set; }

    public ErrorResponse(int statusCode, string? message = null, string? details = null)
    {
        StatusCode = statusCode;
        Message = string.IsNullOrEmpty(message) ? GetDefaultMessage(statusCode) : message;
        Details = details;
    }

    public ErrorResponse(string message, string? details = null)
    {
        Message = message;
        Details = details;
    }

    public static string GetDefaultMessage(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "We don't talk anymore",
            StatusCodes.Status401Unauthorized => "Wait a minute, Who are you",
            StatusCodes.Status403Forbidden => "You shall not pass",
            StatusCodes.Status404NotFound => "Have you seen my cat any where?",
            StatusCodes.Status500InternalServerError => "Internal Server Error",
            _ => "Please report to our support immediately"
        };
    }
}