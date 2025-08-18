using Domain.Entities;
using System.Security.Claims;

namespace Application.Services;

public interface ITokenService
{
    string CreateToken(Usuario usuario, IEnumerable<Claim>? extraClaims = null);
}