using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace University_backend;

public static class JwtHelper
{
    private static readonly SymmetricSecurityKey _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfig.JwtKey));
    private static readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();

    // Get a new JWT token for a given user:
    public static string GetNewToken(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        if (user.Role == null)
        {
            throw new ArgumentNullException(nameof(user.Role), "User's Role cannot be null.");
        }

        // Create a simplified user object (without password) for the JWT payload:
        var slimUser = new { user.Id, user.Name, user.Email, user.RoleId, Role = user.Role.RoleName };
        string json = JsonSerializer.Serialize(slimUser, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        // Claims:
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Actor, json), // Enter user object
            new Claim(ClaimTypes.Role, user.Role.RoleName) // Enter user role
        };

        // Security token descriptor:
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(AppConfig.JwtExpireHours), // Set expiration based on the app's configuration
            SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512)
        };

        // Create and return the JWT token:
        SecurityToken securityToken = _handler.CreateToken(descriptor);
        string token = _handler.WriteToken(securityToken);
        return token;
    }

    // Set default bearer options for validating the JWT token:
    public static void SetBearerOptions(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // We didn't set an Issuer claim (which server/microservice issued the token), so don't validate it.
            ValidateAudience = false, // We didn't set an Audience claim (which server our audience should browse to), so don't validate it.
            ValidateIssuerSigningKey = true, // Validate the secret key.
            IssuerSigningKey = _symmetricSecurityKey // The secret key to validate.
        };
    }
}
