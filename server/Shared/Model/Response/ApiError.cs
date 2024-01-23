using System.Text.Json.Serialization;

namespace Ecommerce.Shared.Model.Response;

public class ApiError : BaseError
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public string? Details { get; set; }

    public ApiError(int statusCode) : base(statusCode)
    {
    }

    public ApiError(int statusCode, string details) : base(statusCode)
    {
        Details = details;
    }

    public ApiError(string message) : base(message)
    {
    }

    public ApiError(string message, string details) : base(message)
    {
        Details = details;
    }
}