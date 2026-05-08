using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogic.Interfaces;
using Domain.DTOs;

namespace BusinessLogic.Core;

public class KeywordService : IKeywordService
{
    private const string BaseUrl = "https://api.searchads.apple.com/api/v5";

    private readonly IAppleSearchAdsCredentialService _credentialService;
    private readonly IHttpClientFactory _httpClientFactory;

    public KeywordService(
        IAppleSearchAdsCredentialService credentialService,
        IHttpClientFactory httpClientFactory)
    {
        _credentialService = credentialService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<KeywordDto>> GetAllAsync(long campaignId, long adGroupId, Guid userId, int? limit = null, int? offset = null, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return Array.Empty<KeywordDto>();

        var url = $"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}/targetingkeywords";
        if (limit.HasValue || offset.HasValue)
        {
            var query = new List<string>();
            if (limit.HasValue) query.Add($"limit={limit.Value}");
            if (offset.HasValue) query.Add($"offset={offset.Value}");
            url += "?" + string.Join("&", query);
        }

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return Array.Empty<KeywordDto>();
        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var response = await client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode) return Array.Empty<KeywordDto>();

        var json = await response.Content.ReadAsStringAsync(ct);
        var list = JsonSerializer.Deserialize<KeywordListResponseDto>(json);
        if (list?.Data != null && list.Data.Count > 0) return list.Data;
        var array = JsonSerializer.Deserialize<List<KeywordDto>>(json);
        return array ?? new List<KeywordDto>();
    }

    public async Task<KeywordDto?> GetByIdAsync(long campaignId, long adGroupId, long keywordId, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return null;
        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var response = await client.GetAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}/targetingkeywords/{keywordId}", ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeKeyword(json);
    }

    public async Task<KeywordDto?> CreateAsync(long campaignId, long adGroupId, CreateKeywordDto dto, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return null;
        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var response = await client.PostAsJsonAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}/targetingkeywords/bulk", new[] { dto }, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        var list = JsonSerializer.Deserialize<KeywordListResponseDto>(json);
        return list?.Data?.FirstOrDefault();
    }

    public async Task<KeywordDto?> UpdateAsync(long campaignId, long adGroupId, UpdateKeywordDto dto, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return null;
        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var response = await client.PutAsJsonAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}/targetingkeywords/bulk", new[] { dto }, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        var list = JsonSerializer.Deserialize<KeywordListResponseDto>(json);
        return list?.Data?.FirstOrDefault();
    }

    public async Task<bool> DeleteAsync(long campaignId, long adGroupId, long keywordId, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return false;

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return false;
        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var request = new HttpRequestMessage(HttpMethod.Post,
            $"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}/targetingkeywords/delete/bulk")
        {
            Content = JsonContent.Create(new[] { keywordId })
        };
        var response = await client.SendAsync(request, ct);
        return response.IsSuccessStatusCode;
    }

    private static KeywordDto? DeserializeKeyword(string json)
    {
        var wrapper = JsonSerializer.Deserialize<KeywordResponseDto>(json);
        if (wrapper?.Data != null) return wrapper.Data;
        return JsonSerializer.Deserialize<KeywordDto>(json);
    }

    public async Task<KeywordReportResponseDto?> GetKeywordReportAsync(long campaignId, Guid userId, KeywordReportRequestDto request, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return null;
        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var response = await client.PostAsJsonAsync($"{BaseUrl}/reports/campaigns/{campaignId}/keywords", request, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<KeywordReportResponseDto>(json);
    }

    private async Task<string?> GetAccessTokenAsync(Guid userId, CancellationToken ct)
    {
        var result = await _credentialService.GetOrCreateAccessToken(userId, ct);
        return result?.AccessToken;
    }

    private HttpClient CreateClient(string bearerToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
        return client;
    }

    private async Task<long?> GetFirstOrgIdAsync(HttpClient client, CancellationToken ct)
    {
        var response = await client.GetAsync($"{BaseUrl}/acls", ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
            return null;

        foreach (var org in data.EnumerateArray())
        {
            if (org.TryGetProperty("orgId", out var orgIdEl) && orgIdEl.TryGetInt64(out var orgId))
                return orgId;
        }
        return null;
    }
}
