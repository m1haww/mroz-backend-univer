using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface ICampaignService
{
    Task<CampaignDto?> GetByIdAsync(long campaignId, Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<CampaignDto>> GetAllAsync(Guid userId, CancellationToken ct = default);
    Task<CampaignDto?> CreateAsync(CreateCampaignDto dto, Guid userId, CancellationToken ct = default);
    Task<CampaignDto?> UpdateAsync(long campaignId, UpdateCampaignDto dto, Guid userId, CancellationToken ct = default);
    Task<bool> DeleteAsync(long campaignId, Guid userId, CancellationToken ct = default);
}
