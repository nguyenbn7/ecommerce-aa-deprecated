using Ecommerce.Module.Accounts.Model;

namespace Ecommerce.Shared.Service;

public interface AuthenticationTokenService
{
    string CreateAccessToken(AppUser user);
}