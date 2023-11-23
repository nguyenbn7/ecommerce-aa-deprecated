using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.API.Accounts;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public Address? Address { get; set; }
}

public class Address
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }

    public string? AppUserId { get; set; }
    public ApplicationUser? AppUser { get; set; }
}

public class AddressDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
}

public class LoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterDTO
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

public class UserDTO
{
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string? Token { get; set; }
}