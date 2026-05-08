using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto, CancellationToken ct)
    {
        var response = await _authService.Register(requestDto, ct);
        if (response == null)
            return BadRequest(new { message = "An account with this email already exists." });

        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto, CancellationToken ct)
    {
        var response = await _authService.Login(requestDto, ct);
        if (response == null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await _authService.GetUserById(userId, ct);
        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name
        });
    }
}
