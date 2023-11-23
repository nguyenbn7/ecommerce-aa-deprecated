using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.API.Accounts;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Share.Service;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration config;
    private readonly SymmetricSecurityKey key;

    public TokenService(IConfiguration config)
    {
        this.config = config;
        var appKey = config["Token:Key"] ?? throw new Exception("Token:Key not provided");
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appKey));
    }

    public string CreateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.GivenName, user.DisplayName!),
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = config["Token:Issuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
