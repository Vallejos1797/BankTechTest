using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public sealed class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public string CreateToken(Usuario usuario, IEnumerable<Claim>? extraClaims = null)
    {
        if (usuario is null)
            throw new ArgumentNullException(nameof(usuario));

        // 📌 Claims base del usuario
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // ✅ ahora sub = Id
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),   // ✅ redundante pero consistente
            new(ClaimTypes.Name, usuario.Nombre),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Rol?.Nombre ?? "comprador")
        };


        // 📌 Claims adicionales
        if (extraClaims != null)
            claims.AddRange(extraClaims);

        // 📌 Clave de seguridad
        var secretKey = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 📌 Configuración de expiración
        var expireMinutes = double.TryParse(_config["Jwt:ExpireMinutes"], out var minutes) ? minutes : 60;

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
