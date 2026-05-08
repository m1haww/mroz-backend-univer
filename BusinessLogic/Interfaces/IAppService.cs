using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IAppService
{
    Task<IReadOnlyList<AppDto>> GetAllAsync(Guid userId, CancellationToken ct = default);
    Task<AppDto?> GetByIdAsync(long id, Guid userId, CancellationToken ct = default);
    Task<AppDto> CreateAsync(CreateAppDto dto, Guid userId, CancellationToken ct = default);
    Task<AppDto?> UpdateAsync(long id, UpdateAppDto dto, Guid userId, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, Guid userId, CancellationToken ct = default);
}
