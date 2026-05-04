using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogic.Interfaces;
using Domain.DTOs;

namespace BusinessLogic.Services;

public class CampaignService : ICampaignService
{
    private const string BaseUrl = "https://api.searchads.apple.com/api/v5";

    private readonly IAppleSearchAdsCredentialService _credentialService;
    private readonly IHttpClientFactory _httpClientFactory;

    public CampaignService(
        IAppleSearchAdsCredentialService credentialService,
        IHttpClientFactory httpClientFactory)
    {
        _credentialService = credentialService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CampaignDto?> GetByIdAsync(long campaignId, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var response = await client.GetAsync($"{BaseUrl}/campaigns/{campaignId}", ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeCampaign(json);
    }

    public async Task<IReadOnlyList<CampaignDto>> GetAllAsync(Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return Array.Empty<CampaignDto>();

        using var client = CreateClient(token!);
        var response = await client.GetAsync($"{BaseUrl}/campaigns", ct);
        if (!response.IsSuccessStatusCode) return Array.Empty<CampaignDto>();

        var json = await response.Content.ReadAsStringAsync(ct);
        var list = JsonSerializer.Deserialize<CampaignListResponseDto>(json);
        if (list?.Data != null && list.Data.Count > 0) return list.Data;
        var array = JsonSerializer.Deserialize<List<CampaignDto>>(json);
        return array ?? new List<CampaignDto>();
    }

    public async Task<CampaignDto?> CreateAsync(CreateCampaignDto dto, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var response = await client.PostAsJsonAsync($"{BaseUrl}/campaigns", dto, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeCampaign(json);
    }

    public async Task<CampaignDto?> UpdateAsync(long campaignId, UpdateCampaignDto dto, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var response = await client.PutAsJsonAsync($"{BaseUrl}/campaigns/{campaignId}", dto, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        return DeserializeCampaign(json);
    }

    public async Task<bool> DeleteAsync(long campaignId, Guid userId, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return false;

        using var client = CreateClient(token!);
        var response = await client.DeleteAsync($"{BaseUrl}/campaigns/{campaignId}", ct);
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

    private static CampaignDto? DeserializeCampaign(string json)
    {
        var wrapper = JsonSerializer.Deserialize<CampaignResponseDto>(json);
        if (wrapper?.Data != null) return wrapper.Data;
        return JsonSerializer.Deserialize<CampaignDto>(json);
    }
}
