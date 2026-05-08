using DataAccess.Database;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Domain.Entities.Credentials;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Core;

public class AppstoreConnectCredentialService : IAppstoreConnectCredentialService
{
    private readonly AppDbContext _dbContext;

    public AppstoreConnectCredentialService(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<AppStoreConnectStatusResponse> GetStatus(Guid userId, CancellationToken ct)
    {
        var cred = await _dbContext.AppStoreConnectCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cred == null)
        {
            return new AppStoreConnectStatusResponse { Configured = false };
        }

        var suffix = cred.KeyId.Length >= 4
            ? "..." + cred.KeyId[^4..]
            : null;

        return new AppStoreConnectStatusResponse
        {
            Configured = true,
            KeyIdSuffix = suffix,
        };
    }

    public async Task<bool> Save(SaveAppStoreConnectDto dto, Guid userId, CancellationToken ct)
    {
        var keyId = dto.KeyId.Trim();
        var issuerId = dto.IssuerId.Trim();
        var privateKey = dto.PrivateKey.Trim();

        if (string.IsNullOrEmpty(keyId) || string.IsNullOrEmpty(issuerId) || string.IsNullOrEmpty(privateKey))
            return false;

        try
        {
            var existing = await _dbContext.AppStoreConnectCredentials
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);

            if (existing != null)
            {
                existing.KeyId = keyId;
                existing.IssuerId = issuerId;
                existing.PrivateKey = privateKey;
            }
            else
            {
                _dbContext.AppStoreConnectCredentials.Add(new AppStoreConnectCredential
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    KeyId = keyId,
                    IssuerId = issuerId,
                    PrivateKey = privateKey
                });
            }

            await _dbContext.SaveChangesAsync(ct);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(Guid userId, CancellationToken ct)
    {
        var cred = await _dbContext.AppStoreConnectCredentials.FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cred != null)
        {
            _dbContext.AppStoreConnectCredentials.Remove(cred);
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }
}
