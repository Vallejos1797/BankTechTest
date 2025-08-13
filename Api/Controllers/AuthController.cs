using Api.Contracts;
using Api.Services;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    private readonly TokenService _tokens;

    public AuthController(AuthService auth, TokenService tokens)
    {
        _auth = auth; _tokens = tokens;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        // valida mínimamente
        if (string.IsNullOrWhiteSpace(req.Username) ||
            string.IsNullOrWhiteSpace(req.Email) ||
            string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Username, email y password son obligatorios.");

        var id = await _auth.RegisterAsync(req.Username.Trim(), req.Email.Trim(), req.Password, req.Role?.Trim() ?? "user", ct);

        // tras registrar, puedes loguear automáticamente:
        var user = await _auth.GetByIdAsync(id, ct)!;
        var token = _tokens.CreateToken(user!, new[] { new Claim(ClaimTypes.Role, user!.Role) });

        return Created("", new AuthResponse(token, new UserResponse(user!.Id, user.Username, user.Email, user.Role)));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var user = await _auth.ValidateUserAsync(req.Username, req.Password, ct);
        if (user is null) return Unauthorized();

        var token = _tokens.CreateToken(user, new[] { new Claim(ClaimTypes.Role, user.Role) });
        return Ok(new AuthResponse(token, new UserResponse(user.Id, user.Username, user.Email, user.Role)));
    }
}