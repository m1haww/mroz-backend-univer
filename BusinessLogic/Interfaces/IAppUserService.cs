using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IAppUserService
{
    Task<IReadOnlyList<AppUserDto>> GetAllAsync(string? appId, CancellationToken ct = default);
    Task<AppUserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<AppUserDto> CreateAsync(CreateAppUserDto dto, CancellationToken ct = default);
    Task<AppUserDto?> UpdateAsync(Guid id, UpdateAppUserDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
