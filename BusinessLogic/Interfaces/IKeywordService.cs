using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IKeywordService
{
    Task<IReadOnlyList<KeywordDto>> GetAllAsync(long campaignId, long adGroupId, Guid userId, int? limit = null, int? offset = null, CancellationToken ct = default);
    Task<KeywordReportResponseDto?> GetKeywordReportAsync(long campaignId, Guid userId, KeywordReportRequestDto request, CancellationToken ct = default);
}
