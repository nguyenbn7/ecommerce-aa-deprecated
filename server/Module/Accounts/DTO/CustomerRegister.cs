using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Module.Accounts.DTO;

public class CustomerRegister
{
    [Required]
    public required string DisplayName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [RegularExpression(@"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$",
    ErrorMessage = "Password must have 1 uppercase, 1 lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
    public required string Password { get; set; }
}