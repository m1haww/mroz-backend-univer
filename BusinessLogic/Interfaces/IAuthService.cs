using Domain.DTOs;
using Domain.Entities.User;

namespace BusinessLogic.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> Register(RegisterRequestDto requestDto, CancellationToken ct = default);
    Task<AuthResponseDto?> Login(LoginRequestDto requestDto, CancellationToken ct = default);
    Task<User?> GetUserById(Guid id, CancellationToken ct = default);
}
