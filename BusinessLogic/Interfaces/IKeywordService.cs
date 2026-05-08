using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IKeywordService
{
    Task<IReadOnlyList<KeywordDto>> GetAllAsync(long campaignId, long adGroupId, Guid userId, int? limit = null, int? offset = null, CancellationToken ct = default);
    Task<KeywordDto?> GetByIdAsync(long campaignId, long adGroupId, long keywordId, Guid userId, CancellationToken ct = default);
    Task<KeywordDto?> CreateAsync(long campaignId, long adGroupId, CreateKeywordDto dto, Guid userId, CancellationToken ct = default);
    Task<KeywordDto?> UpdateAsync(long campaignId, long adGroupId, UpdateKeywordDto dto, Guid userId, CancellationToken ct = default);
    Task<bool> DeleteAsync(long campaignId, long adGroupId, long keywordId, Guid userId, CancellationToken ct = default);
    Task<KeywordReportResponseDto?> GetKeywordReportAsync(long campaignId, Guid userId, KeywordReportRequestDto request, CancellationToken ct = default);
}
