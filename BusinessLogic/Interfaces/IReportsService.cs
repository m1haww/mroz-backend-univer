using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IReportsService
{
    Task<CampaignReportResponseDto?> GetCampaignReportAsync(Guid userId, CampaignReportRequestDto request, CancellationToken ct = default);
}
