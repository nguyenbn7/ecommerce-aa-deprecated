using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Module.Accounts.DTO;

public class CustomerLogin
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}