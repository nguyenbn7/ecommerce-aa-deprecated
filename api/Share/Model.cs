using System.Text.Json.Serialization;

namespace Ecommerce.Share.Model;

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

public class ValidationErrorResponse : ErrorResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? Errors { get; set; }

    public ValidationErrorResponse(string? message = null, string? details = null)
        : base(StatusCodes.Status400BadRequest, message, details)
    {
    }
}

public class Page<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public required IReadOnlyList<T> Data { get; set; }
}

public class Pageable
{
    public int Index { get; init; }
    public int Size { get; init; }

    public static Pageable Of(int pageIndex, int pageSize)
    {
        return new Pageable
        {
            Index = pageIndex < 0 ? 0 : pageIndex,
            Size = pageSize < 1 ? 6 : pageSize
        };
    }
}