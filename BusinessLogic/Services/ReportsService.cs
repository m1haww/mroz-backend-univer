using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogic.Interfaces;
using Domain.DTOs;

namespace BusinessLogic.Services;

public class ReportsService : IReportsService
{
    private const string BaseUrl = "https://api.searchads.apple.com/api/v5";
    private readonly IAppleSearchAdsCredentialService _credentialService;
    private readonly IHttpClientFactory _httpClientFactory;

    public ReportsService(IAppleSearchAdsCredentialService credentialService, IHttpClientFactory httpClientFactory)
    {
        _credentialService = credentialService;
        _httpClientFactory = httpClientFactory;
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

    public async Task<CampaignReportResponseDto?> GetCampaignReportAsync(Guid userId, CampaignReportRequestDto request, CancellationToken ct = default)
    {
        var token = await GetAccessTokenAsync(userId, ct);
        if (token == null) return null;

        using var client = CreateClient(token!);
        var orgId = await GetFirstOrgIdAsync(client, ct);
        if (orgId == null) return null;

        client.DefaultRequestHeaders.Add("X-AP-Context", $"orgId={orgId}");

        var response = await client.PostAsJsonAsync($"{BaseUrl}/reports/campaigns", request, ct);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("data", out var dataEl) ||
            !dataEl.TryGetProperty("reportingDataResponse", out var reportingEl))
            return null;

        return JsonSerializer.Deserialize<CampaignReportResponseDto>(reportingEl.GetRawText());
    }
}
