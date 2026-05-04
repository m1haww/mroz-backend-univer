using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Database;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;

    public AuthService(IConfiguration config, AppDbContext db)
    {
        _config = config;
        _db = db;
    }

    public async Task<AuthResponseDto?> Register(RegisterRequestDto requestDto, CancellationToken ct = default)
    {
        var emailNorm = requestDto.Email.Trim();
        if (await _db.Users.AnyAsync(u => u.Email == emailNorm, ct))
            return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = emailNorm,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(requestDto.Password),
            Name = requestDto.Name?.Trim() ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return BuildAuthResponse(user);
    }

    public async Task<AuthResponseDto?> Login(LoginRequestDto requestDto, CancellationToken ct = default)
    {
        var emailNorm = requestDto.Email.Trim();
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == emailNorm, ct);
        if (user == null || !BCrypt.Net.BCrypt.Verify(requestDto.Password, user.PasswordHash))
            return null;

        return BuildAuthResponse(user);
    }

    public async Task<User?> GetUserById(Guid id, CancellationToken ct = default)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    private AuthResponseDto BuildAuthResponse(User user)
    {
        var token = GenerateJwt(user);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            }
        };
    }

    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not in appsettings.json")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
