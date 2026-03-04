using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ADC_STD.ApiTests.Helpers;

public static class JwtTokenHelper
{
    public const string TestIssuer = "TestIssuer";
    public const string TestAudience = "TestAudience";
    public const string SecretKey = "your-super-secret-test-key-that-is-32-chars-long!";

    public static string GenerateTestToken(string userId = "test-user-id", string userName = "TestUser", string email = "test@example.com")
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Name, userName),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email)
        };

        var token = new JwtSecurityToken(
            issuer: TestIssuer,
            audience: TestAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static Dictionary<string, string> GetAuthHeaders(string? token = null)
    {
        token ??= GenerateTestToken();
        return new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {token}" }
        };
    }
}
