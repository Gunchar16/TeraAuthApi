using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeraAuthApi.Application.Settings;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Utilities;

public static class JwtUtility
{
    public static string GenerateJwtToken(User user, JwtSettings jwtSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(jwtSettings.Key);
        var role = user.UserRoles?.FirstOrDefault()?.Role?.Name ?? "User"; // Default to "User" role if no role is found
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            }),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(24 * 60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}