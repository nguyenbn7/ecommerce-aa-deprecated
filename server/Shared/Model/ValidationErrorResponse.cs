using System.Text.Json.Serialization;

namespace Ecommerce.Shared.Model;

public class ValidationErrorResponse : ErrorResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? Errors { get; set; }

    public ValidationErrorResponse(string? message = null, string? details = null)
        : base(StatusCodes.Status400BadRequest, message, details)
    {
    }
}