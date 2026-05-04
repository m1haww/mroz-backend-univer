using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BusinessLogic.Database;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Domain.Entities.Credentials;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services;

public class AppleSearchAdsCredentialService : IAppleSearchAdsCredentialService
{
    private const string AppleTokenUrl = "https://appleid.apple.com/auth/oauth2/token";
    private const string AppleAudience = "https://appleid.apple.com";
    private const string SearchAdsScope = "searchadsorg";
    private static readonly TimeSpan TokenRefreshBuffer = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan JwtRefreshBuffer = TimeSpan.FromDays(1);
    private static readonly TimeSpan JwtMaxLifetime = TimeSpan.FromDays(180);

    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public AppleSearchAdsCredentialService(AppDbContext dbContext, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AppleSearchAdsStatusResponse> GetStatus(Guid userId, CancellationToken ct)
    {
        var cred = await _dbContext.AppleSearchAdsCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cred == null)
            return new AppleSearchAdsStatusResponse { Configured = false };

        var suffix = cred.KeyId.Length >= 4 ? "..." + cred.KeyId[^4..] : null;
        return new AppleSearchAdsStatusResponse { Configured = true, KeyIdSuffix = suffix };
    }

    public async Task<Guid?> AddCredential(Guid userId, AddAppleSearchAdsDto dto, CancellationToken ct)
    {
        var clientId = dto.ClientId?.Trim() ?? string.Empty;
        var teamId = dto.TeamId?.Trim() ?? string.Empty;
        var keyId = dto.KeyId?.Trim() ?? string.Empty;
        var privateKey = dto.PrivateKey?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(teamId) || string.IsNullOrEmpty(keyId) || string.IsNullOrEmpty(privateKey))
            return null;

        var existing = await _dbContext.AppleSearchAdsCredentials
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (existing != null)
        {
            existing.PrivateKey = privateKey;
            existing.PublicKey = dto.PublicKey?.Trim() ?? string.Empty;
            existing.ClientId = clientId;
            existing.TeamId = teamId;
            existing.KeyId = keyId;
            existing.ClientSecretJwt = null;
            existing.ClientSecretJwtExpiresAt = null;
            existing.AccessToken = null;
            existing.AccessTokenExpiresAt = null;
        }
        else
        {
            var cred = new AppleSearchAdsCredential
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PrivateKey = privateKey,
                PublicKey = dto.PublicKey?.Trim() ?? string.Empty,
                ClientId = clientId,
                TeamId = teamId,
                KeyId = keyId,
                Alg = "ES256"
            };
            _dbContext.AppleSearchAdsCredentials.Add(cred);
        }

        await _dbContext.SaveChangesAsync(ct);
        var current = await _dbContext.AppleSearchAdsCredentials
            .FirstAsync(c => c.UserId == userId, ct);
        return current.Id;
    }

    public async Task<AppleSearchAdsAccessTokenResult?> GetOrCreateAccessToken(Guid userId, CancellationToken ct)
    {
        var cred = await _dbContext.AppleSearchAdsCredentials
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
        if (cred == null)
            return null;

        var now = DateTime.UtcNow;
        if (cred.AccessToken != null && cred.AccessTokenExpiresAt.HasValue &&
            cred.AccessTokenExpiresAt.Value > now + TokenRefreshBuffer)
        {
            return new AppleSearchAdsAccessTokenResult
            {
                AccessToken = cred.AccessToken,
                ExpiresAt = cred.AccessTokenExpiresAt.Value
            };
        }

        var clientSecretJwt = cred.ClientSecretJwt;
        if (string.IsNullOrEmpty(clientSecretJwt) || !cred.ClientSecretJwtExpiresAt.HasValue ||
            cred.ClientSecretJwtExpiresAt.Value <= now + JwtRefreshBuffer)
        {
            clientSecretJwt = CreateClientSecretJwt(cred);
            if (string.IsNullOrEmpty(clientSecretJwt))
                return null;
            cred.ClientSecretJwt = clientSecretJwt;
            cred.ClientSecretJwtExpiresAt = now.Add(JwtMaxLifetime);
            await _dbContext.SaveChangesAsync(ct);
        }

        var tokenResult = await ExchangeForAccessToken(cred.ClientId, clientSecretJwt, ct);
        if (tokenResult == null)
            return null;

        cred.AccessToken = tokenResult.AccessToken;
        cred.AccessTokenExpiresAt = tokenResult.ExpiresAt;
        await _dbContext.SaveChangesAsync(ct);

        return tokenResult;
    }

    public async Task<bool> Delete(Guid userId, CancellationToken ct)
    {
        var cred = await _dbContext.AppleSearchAdsCredentials
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
        if (cred == null)
            return false;
        _dbContext.AppleSearchAdsCredentials.Remove(cred);
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    private static string? CreateClientSecretJwt(AppleSearchAdsCredential cred)
    {
        try
        {
            using var ecdsa = ECDsa.Create();
            ecdsa.ImportFromPem(cred.PrivateKey);

            var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var exp = DateTimeOffset.UtcNow.Add(JwtMaxLifetime).ToUnixTimeSeconds();

            var header = new Dictionary<string, string>
            {
                ["alg"] = "ES256",
                ["kid"] = cred.KeyId
            };
            var payload = new Dictionary<string, object>
            {
                ["sub"] = cred.ClientId,
                ["aud"] = AppleAudience,
                ["iat"] = iat,
                ["exp"] = exp,
                ["iss"] = cred.TeamId
            };

            var headerB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header)));
            var payloadB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));
            var signingInput = $"{headerB64}.{payloadB64}";
            var signingInputBytes = Encoding.UTF8.GetBytes(signingInput);

            var signatureDer = ecdsa.SignData(signingInputBytes, HashAlgorithmName.SHA256, DSASignatureFormat.Rfc3279DerSequence);
            var signatureRaw = DerToRawEcdsaSignature(signatureDer, 32);
            var signatureB64 = Base64UrlEncode(signatureRaw);

            return $"{signingInput}.{signatureB64}";
        }
        catch
        {
            return null;
        }
    }

    private static string Base64UrlEncode(byte[] data)
    {
        return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    private static byte[] DerToRawEcdsaSignature(byte[] der, int componentSize)
    {
        if (der.Length < 8 || der[0] != 0x30) return Array.Empty<byte>();
        var idx = 2;
        if (der[1] == 0x81) idx += 1;
        else if (der[1] == 0x82) idx += 2;

        if (der[idx++] != 0x02) return Array.Empty<byte>();
        var rLen = der[idx++];
        if (rLen > 33) return Array.Empty<byte>();
        var r = der.AsSpan(idx, rLen);
        idx += rLen;
        if (der[idx++] != 0x02) return Array.Empty<byte>();
        var sLen = der[idx++];
        if (sLen > 33) return Array.Empty<byte>();
        var s = der.AsSpan(idx, sLen);

        var result = new byte[componentSize * 2];
        CopyPadded(r, result.AsSpan(0, componentSize));
        CopyPadded(s, result.AsSpan(componentSize, componentSize));
        return result;
    }

    private static void CopyPadded(ReadOnlySpan<byte> value, Span<byte> target)
    {
        if (value.Length == target.Length + 1 && value[0] == 0)
            value = value.Slice(1);
        var pad = target.Length - value.Length;
        value.CopyTo(target.Slice(pad));
    }

    private async Task<AppleSearchAdsAccessTokenResult?> ExchangeForAccessToken(string clientId, string clientSecretJwt, CancellationToken ct)
    {
        using var client = _httpClientFactory.CreateClient();
        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecretJwt,
            ["scope"] = SearchAdsScope
        };
        using var content = new FormUrlEncodedContent(form);

        var response = await client.PostAsync(AppleTokenUrl, content, ct);
        var json = await response.Content.ReadAsStringAsync(ct);
        if (!response.IsSuccessStatusCode)
            return null;

        var tokenResponse = JsonSerializer.Deserialize<AppleTokenEndpointResponse>(json);
        if (tokenResponse?.AccessToken == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            return null;

        var expiresAt = DateTime.UtcNow.AddHours(1);
        if (tokenResponse.ExpiresIn.HasValue && tokenResponse.ExpiresIn.Value > 0)
            expiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn.Value);

        return new AppleSearchAdsAccessTokenResult
        {
            AccessToken = tokenResponse.AccessToken,
            TokenType = tokenResponse.TokenType ?? "Bearer",
            ExpiresAt = expiresAt
        };
    }
}
