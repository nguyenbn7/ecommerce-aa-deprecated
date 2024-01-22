using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Module.Accounts.Model;

public class AppUserRole : IdentityUserRole<string>
{
    public required AppUser User { get; set; }
    public required AppRole Role { get; set; }
}