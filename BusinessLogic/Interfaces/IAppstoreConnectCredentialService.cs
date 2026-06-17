using Domain.DTOs;

namespace BusinessLogic.Interfaces;

public interface IAppstoreConnectCredentialService
{
    Task<AppStoreConnectStatusResponse> GetStatus(Guid userId, CancellationToken ct);
    Task<bool> Save(SaveAppStoreConnectDto dto, Guid userId, CancellationToken ct);
    Task<bool> Delete(Guid userId, CancellationToken ct);
}
