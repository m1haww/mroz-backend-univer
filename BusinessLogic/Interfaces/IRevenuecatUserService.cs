using Domain.DTOs;
using Domain.Entities.App;

namespace BusinessLogic.Interfaces;

public interface IRevenuecatUserService
{
    Task<AppUser?> SetUserAsync(AddRevenuecatUserDto dto, CancellationToken ct = default);
}
