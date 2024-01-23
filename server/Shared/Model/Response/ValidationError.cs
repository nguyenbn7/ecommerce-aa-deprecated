using System.Text.Json.Serialization;

namespace Ecommerce.Shared.Model.Response;

public class ValidationError : BaseError
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? Errors { get; set; }

    public ValidationError(int statusCode) : base(statusCode)
    {
    }

    public ValidationError(int statusCode, IEnumerable<string> errors) : base(statusCode)
    {
        Errors = errors;
    }

    public ValidationError(string message) : base(message)
    {
    }

    public ValidationError(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}