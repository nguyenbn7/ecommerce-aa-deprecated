using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Module.Accounts.Model;

public class AppRole : IdentityRole
{
    public List<AppUserRole> UserRoles { get; set; } = new();
}