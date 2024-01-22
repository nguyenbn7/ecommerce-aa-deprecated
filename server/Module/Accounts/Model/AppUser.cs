using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Module.Accounts.Model;

public class AppUser : IdentityUser
{
    public required string DisplayName { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;

    public Address? Address { get; set; }
    public HashSet<AppUserRole> UserRoles { get; set; } = new();
}