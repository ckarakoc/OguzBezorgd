using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Services;

public class TokenService(IConfiguration config, UserManager<User> userManager) : ITokenService
{
    public async Task<string> GenerateTokenAsync(User user, DateTime expireTime)
    {
        var secret = config["JwtSettings:SecretKey"] ?? throw new Exception("the token key is missing");
        if (secret.Length < 64) throw new Exception("TokenKey must be at least 64 characters long");
        var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? throw new Exception("No username for user")),
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expireTime,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static long GetTokenExpirationTime(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
        var ticks = long.Parse(tokenExp);
        return ticks;
    }

    public async Task<bool> CheckTokenIsValid(string token)
    {
        var tokenKey = config["JwtSettings:SecretKey"] ?? throw new Exception("the token key is missing");

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Checks the Expiration and NotBefore claims
            ValidateIssuerSigningKey = true,

            // These are values you use to validate the token's issuer and audience
            // ValidIssuer = config["JwtSettings:Issuer"],   // e.g., "yourdomain.com"
            // ValidAudience = config["JwtSettings:Audience"], // e.g., "yourapi"

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var result = await tokenHandler.ValidateTokenAsync(token, validationParameters);
        return result.IsValid;
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32]; // 256-bit token
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}