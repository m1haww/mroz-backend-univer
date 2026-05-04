using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogic.Interfaces;
using Domain.DTOs;

namespace BusinessLogic.Services;

public class AdGroupService : IAdGroupService
{
    private const string BaseUrl = "https://api.searchads.apple.com/api/v5";

    private readonly IAppleSearchAdsCredentialService _credentialService;
    private readonly IHttpClientFactory _httpClientFactory;

    public AdGroupService(
        IAppleSearchAdsCredentialService credentialService,
        IHttpClientFactory httpClientFactory)
    {
        _credentialService = credentialService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AdGroupDto?> GetByIdAsync(long campaignId, long adGroupId, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var response = await client.GetAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}", ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeAdGroup(json);
    }

    public async Task<IReadOnlyList<AdGroupDto>> GetAllAsync(long campaignId, Guid userId, int? limit = null, int? offset = null, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return Array.Empty<AdGroupDto>();

        var url = $"{BaseUrl}/campaigns/{campaignId}/adgroups";
        if (limit.HasValue || offset.HasValue)
        {
            var query = new List<string>();
            if (limit.HasValue) query.Add($"limit={limit.Value}");
            if (offset.HasValue) query.Add($"offset={offset.Value}");
            url += "?" + string.Join("&", query);
        }

        using var client = CreateClient(token!);
        var response = await client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode) return Array.Empty<AdGroupDto>();

        var json = await response.Content.ReadAsStringAsync(ct);
        var list = JsonSerializer.Deserialize<AdGroupListResponseDto>(json);
        if (list?.Data != null && list.Data.Count > 0) return list.Data;
        var array = JsonSerializer.Deserialize<List<AdGroupDto>>(json);
        return array ?? new List<AdGroupDto>();
    }

    public async Task<AdGroupDto?> CreateAsync(long campaignId, CreateAdGroupDto dto, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var response = await client.PostAsJsonAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups", dto, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeAdGroup(json);
    }

    public async Task<AdGroupDto?> UpdateAsync(long campaignId, long adGroupId, UpdateAdGroupDto dto, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var response = await client.PutAsJsonAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}", dto, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeAdGroup(json);
    }

    public async Task<bool> DeleteAsync(long campaignId, long adGroupId, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return false;

        using var client = CreateClient(token!);
        var response = await client.DeleteAsync($"{BaseUrl}/campaigns/{campaignId}/adgroups/{adGroupId}", ct);
        return response.IsSuccessStatusCode;
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

    private static AdGroupDto? DeserializeAdGroup(string json)
    {
        var wrapper = JsonSerializer.Deserialize<AdGroupResponseDto>(json);
        if (wrapper?.Data != null) return wrapper.Data;
        return JsonSerializer.Deserialize<AdGroupDto>(json);
    }
}
