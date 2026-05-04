using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IAdGroupService
{
    Task<AdGroupDto?> GetByIdAsync(long campaignId, long adGroupId, Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<AdGroupDto>> GetAllAsync(long campaignId, Guid userId, int? limit = null, int? offset = null, CancellationToken ct = default);
    Task<AdGroupDto?> CreateAsync(long campaignId, CreateAdGroupDto dto, Guid userId, CancellationToken ct = default);
    Task<AdGroupDto?> UpdateAsync(long campaignId, long adGroupId, UpdateAdGroupDto dto, Guid userId, CancellationToken ct = default);
    Task<bool> DeleteAsync(long campaignId, long adGroupId, Guid userId, CancellationToken ct = default);
}
