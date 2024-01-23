namespace Ecommerce.Shared.Model.Response;

public abstract class BaseError
{
    public string Message { get; set; }

    protected BaseError(int statusCode)
    {
        Message = GetDefaultMessage(statusCode);
    }

    protected BaseError(string message)
    {
        Message = message;
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