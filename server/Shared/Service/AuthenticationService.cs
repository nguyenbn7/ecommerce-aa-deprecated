using Ecommerce.Module.Accounts.Model;

namespace Ecommerce.Shared.Service;

public interface AuthenticationService
{
    string CreateAccessToken(AppUser user);
}