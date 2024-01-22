namespace Ecommerce.Module.Accounts.Model.Response;

public class CustomerInfoWithToken
{
    public string? Email { get; set; }
    public required string DisplayName { get; set; }
    public required string Token { get; set; }
}