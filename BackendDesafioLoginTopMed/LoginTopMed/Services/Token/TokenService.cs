using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginTopMed.Services.Token;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(int userId, string username)
    {
        var jwtSettings = _config.GetSection("JwtSettings");

        var secretKeyValue = jwtSettings["SecretKey"];
        if (string.IsNullOrEmpty(secretKeyValue))
        {
            throw new InvalidOperationException(
                "JWT SecretKey não está configurada. " +
                "Configure usando: dotnet user-secrets set \"JwtSettings:SecretKey\" \"your-key\""
            );
        }

        var secretKey = Encoding.ASCII.GetBytes(secretKeyValue);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var accessTokenExpireMinutesValue = jwtSettings["AccessTokenExpireMinutes"];
        if (!int.TryParse(accessTokenExpireMinutesValue, out int accessTokenExpireMinutes))
        {
            accessTokenExpireMinutes = 60;
        }

        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        if (string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException(
                "JWT Issuer não está configurado. " +
                "Configure usando: dotnet user-secrets set \"JwtSettings:Issuer\" \"your-issuer\""
            );
        }

        if (string.IsNullOrEmpty(audience))
        {
            throw new InvalidOperationException(
                "JWT Audience não está configurado. " +
                "Configure usando: dotnet user-secrets set \"JwtSettings:Audience\" \"your-audience\""
            );
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(accessTokenExpireMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
