using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using DataAccess.Database;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Core;

public class ReportsService : IReportsService
{
    private const string BaseUrl = "https://api.searchads.apple.com/api/v5";
    private readonly IAppleSearchAdsCredentialService _credentialService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _db;

    public ReportsService(
        IAppleSearchAdsCredentialService credentialService,
        IHttpClientFactory httpClientFactory,
        AppDbContext db)
    {
        _credentialService = credentialService;
        _httpClientFactory = httpClientFactory;
        _db = db;
    }

    public async Task<CampaignReportResponseDto?> GetCampaignReportAsync(Guid userId, CampaignReportRequestDto request, CancellationToken ct = default)
    {
        var report = await FetchCampaignReportAsync(userId, request, ct);
        if (report?.Data?.ReportingDataResponse?.Row == null)
            return report;

        if (!TryParseDateRange(request.StartTime, request.EndTime, out var startUtc, out var endUtc))
            return report;

        foreach (var row in report.Data.ReportingDataResponse.Row)
        {
            var campaignId = row.Metadata?.CampaignId;
            if (!campaignId.HasValue) continue;

            var (revenue, trialsCount, payingUserCount) =
                await GetRevenueAndCountsAsync(campaignId.Value, startUtc, endUtc, ct);

            row.Revenue = (decimal)revenue;
            row.TrialsCount = trialsCount;

            var spend = AggregateSpend(row);
            var installs = AggregateInstalls(row);

            row.Arpu = installs > 0 ? (decimal)revenue / installs : 0;
            row.Trial2PaidConversionRate = trialsCount > 0 ? (double)payingUserCount / trialsCount * 100.0 : 0;
            row.Install2TrialConversionRate = installs > 0 ? (double)trialsCount / installs * 100.0 : 0;
            row.Install2PaidConversionRate = installs > 0 ? (double)payingUserCount / installs * 100.0 : 0;

            if (spend > 0)
            {
                row.Roas = row.Revenue / spend;
                row.Cac = payingUserCount > 0 ? spend / payingUserCount : 0;
                row.CostPerTrial = trialsCount > 0 ? spend / trialsCount : 0;
            }
            else
            {
                row.Roas = 0;
                row.Cac = 0;
                row.CostPerTrial = 0;
            }
        }

        return report;
    }

    private async Task<CampaignReportResponseDto?> FetchCampaignReportAsync(Guid userId, CampaignReportRequestDto request, CancellationToken ct)
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
        return JsonSerializer.Deserialize<CampaignReportResponseDto>(json);
    }

    private async Task<(double Revenue, int TrialsCount, int PayingUserCount)> GetRevenueAndCountsAsync(long campaignId, DateTime startUtc, DateTime endUtc, CancellationToken ct)
    {
        var query = _db.AppUsers
            .AsNoTracking()
            .Where(u => u.CampaignId == campaignId && u.InstallDate >= startUtc && u.InstallDate <= endUtc);

        var revenue = await query.SumAsync(u => u.TotalRevenue, ct);
        var trialsCount = await query.CountAsync(u => u.HasTrial, ct);
        var payingUserCount = await query.CountAsync(u => u.TotalRevenue > 0, ct);
        return (revenue, trialsCount, payingUserCount);
    }

    private static decimal AggregateSpend(CampaignReportRowDto row)
    {
        if (row.Granularity is { Count: > 0 } buckets)
            return buckets.Sum(g => ParseAmount(g.LocalSpend?.Amount));
        return ParseAmount(row.Total?.LocalSpend?.Amount);
    }

    private static int AggregateInstalls(CampaignReportRowDto row)
    {
        if (row.Granularity is { Count: > 0 } buckets)
            return buckets.Sum(g => g.TotalInstalls ?? 0);
        return row.Total?.TotalInstalls ?? 0;
    }

    private static decimal ParseAmount(string? amount)
    {
        if (string.IsNullOrWhiteSpace(amount)) return 0;
        return decimal.TryParse(amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
    }

    private static bool TryParseDateRange(string? start, string? end, out DateTime startUtc, out DateTime endUtc)
    {
        startUtc = default; endUtc = default;
        if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end)) return false;
        if (!DateTime.TryParse(start, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out startUtc)) return false;
        if (!DateTime.TryParse(end, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out endUtc)) return false;
        startUtc = startUtc.Date;
        endUtc = endUtc.Date.AddDays(1).AddTicks(-1);
        return true;
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
