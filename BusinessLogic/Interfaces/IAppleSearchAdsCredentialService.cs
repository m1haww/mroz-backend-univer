using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IAppleSearchAdsCredentialService
{
    Task<AppleSearchAdsStatusResponse> GetStatus(Guid userId, CancellationToken ct);
    Task<Guid?> AddCredential(Guid userId, AddAppleSearchAdsDto dto, CancellationToken ct);
    Task<AppleSearchAdsAccessTokenResult?> GetOrCreateAccessToken(Guid userId, CancellationToken ct);
    Task<bool> Delete(Guid userId, CancellationToken ct);
}
