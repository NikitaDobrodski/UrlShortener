using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Abstractions;
using UrlShortener.Application.Abstractions;
using UrlShortener.Core.Entities;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly ISecurityService _security;
    private readonly ITokenService _tokens;

    public AuthController(IUserRepository users, ISecurityService security, ITokenService tokens)
    {
        _users = users;
        _security = security;
        _tokens = tokens;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _users.ExistsByEmailAsync(request.Email))
            return BadRequest("User already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower(),
            PasswordHash = _security.HashPassword(request.Password),
        };

        await _users.AddAsync(user);

        return Ok("Registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email.ToLower());
        if (user is null)
            return Unauthorized("Invalid email or password.");

        if (!_security.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        var token = _tokens.GenerateToken(user.Id, user.Email);

        return Ok(new { token });
    }
}

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);
