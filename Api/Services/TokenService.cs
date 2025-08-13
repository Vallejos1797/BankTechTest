// Api/Services/TokenService.cs
using Domain.Entities;                    // si quieres pasar el User completo
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public sealed class TokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config) => _config = config;

    // Versión que recibe el usuario de dominio
    public string CreateToken(User user, IEnumerable<Claim>? extraClaims = null)
    {
        var baseClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role ?? "user")
        };

        if (extraClaims is not null) baseClaims.AddRange(extraClaims);

        return Create(baseClaims);
    }

    // Versión compatible con tu firma anterior (por si no quieres referenciar Domain aquí)
    public string CreateToken(string username, string? email = null, string role = "user", IEnumerable<Claim>? extraClaims = null)
    {
        var baseClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role)
        };
        if (!string.IsNullOrWhiteSpace(email))
            baseClaims.Add(new Claim(ClaimTypes.Email, email));

        if (extraClaims is not null) baseClaims.AddRange(extraClaims);

        return Create(baseClaims);
    }

    private string Create(IEnumerable<Claim> claims)
    {
        var issuer  = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;
        var minutes = double.TryParse(_config["Jwt:ExpireMinutes"], out var m) ? m : 60d;

        // La clave debe ser larga (>=32 chars). En prod guárdala en variables de entorno/KeyVault.
        var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
