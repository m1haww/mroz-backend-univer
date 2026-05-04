using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogic.Interfaces;
using Domain.DTOs;

namespace BusinessLogic.Services;

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
        var response = await client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode) return Array.Empty<KeywordDto>();

        var json = await response.Content.ReadAsStringAsync(ct);
        var list = JsonSerializer.Deserialize<KeywordListResponseDto>(json);
        if (list?.Data != null && list.Data.Count > 0) return list.Data;
        var array = JsonSerializer.Deserialize<List<KeywordDto>>(json);
        return array ?? new List<KeywordDto>();
    }

    public async Task<KeywordReportResponseDto?> GetKeywordReportAsync(long campaignId, Guid userId, KeywordReportRequestDto request, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
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
}
