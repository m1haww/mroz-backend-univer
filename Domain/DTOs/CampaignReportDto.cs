using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class CampaignReportRequestDto
{
    [JsonPropertyName("startTime")]
    public string? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public string? EndTime { get; set; }

    [JsonPropertyName("timeZone")]
    public string? TimeZone { get; set; }

    [JsonPropertyName("granularity")]
    public string? Granularity { get; set; }

    [JsonPropertyName("returnRowTotals")]
    public bool? ReturnRowTotals { get; set; }

    [JsonPropertyName("returnGrandTotals")]
    public bool? ReturnGrandTotals { get; set; }

    [JsonPropertyName("returnRecordsWithNoMetrics")]
    public bool? ReturnRecordsWithNoMetrics { get; set; }

    [JsonPropertyName("groupBy")]
    public List<string>? GroupBy { get; set; }

    [JsonPropertyName("selector")]
    public CampaignReportSelectorDto? Selector { get; set; }
}

public class CampaignReportSelectorDto
{
    [JsonPropertyName("orderBy")]
    public List<CampaignReportOrderByDto>? OrderBy { get; set; }

    [JsonPropertyName("conditions")]
    public List<CampaignReportConditionDto>? Conditions { get; set; }

    [JsonPropertyName("pagination")]
    public CampaignReportPaginationDto? Pagination { get; set; }
}

public class CampaignReportOrderByDto
{
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("sortOrder")]
    public string? SortOrder { get; set; }
}

public class CampaignReportConditionDto
{
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("operator")]
    public string? Operator { get; set; }

    [JsonPropertyName("values")]
    public List<string>? Values { get; set; }
}

public class CampaignReportPaginationDto
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}

public class CampaignReportResponseDto
{
    [JsonPropertyName("row")]
    public List<CampaignReportRowDto>? Row { get; set; }

    [JsonPropertyName("grandTotals")]
    public CampaignReportGrandTotalsDto? GrandTotals { get; set; }
}

public class CampaignReportRowDto
{
    [JsonPropertyName("granularity")]
    public List<string>? Granularity { get; set; }

    [JsonPropertyName("metadata")]
    public CampaignReportMetadataDto? Metadata { get; set; }

    [JsonPropertyName("total")]
    public KeywordReportTotalDto? Total { get; set; }

    [JsonPropertyName("other")]
    public bool Other { get; set; }
}

public class CampaignReportGrandTotalsDto
{
    [JsonPropertyName("other")]
    public bool Other { get; set; }

    [JsonPropertyName("total")]
    public KeywordReportTotalDto? Total { get; set; }
}

public class CampaignReportMetadataDto
{
    [JsonPropertyName("adChannelType")]
    public string? AdChannelType { get; set; }

    [JsonPropertyName("billingEvent")]
    public string? BillingEvent { get; set; }

    [JsonPropertyName("biddingStrategy")]
    public string? BiddingStrategy { get; set; }

    [JsonPropertyName("targetCpa")]
    public MoneyDto? TargetCpa { get; set; }

    [JsonPropertyName("supplySources")]
    public List<string>? SupplySources { get; set; }

    [JsonPropertyName("campaignId")]
    public long? CampaignId { get; set; }

    [JsonPropertyName("campaignName")]
    public string? CampaignName { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("campaignStatus")]
    public string? CampaignStatus { get; set; }

    [JsonPropertyName("app")]
    public CampaignReportAppDto? App { get; set; }

    [JsonPropertyName("servingStatus")]
    public string? ServingStatus { get; set; }

    [JsonPropertyName("servingStateReasons")]
    public List<string>? ServingStateReasons { get; set; }

    [JsonPropertyName("countriesOrRegions")]
    public List<string>? CountriesOrRegions { get; set; }

    [JsonPropertyName("modificationTime")]
    public string? ModificationTime { get; set; }

    [JsonPropertyName("totalBudget")]
    public MoneyDto? TotalBudget { get; set; }

    [JsonPropertyName("dailyBudgetAmount")]
    public MoneyDto? DailyBudgetAmount { get; set; }

    [JsonPropertyName("displayStatus")]
    public string? DisplayStatus { get; set; }

    [JsonPropertyName("orgId")]
    public long? OrgId { get; set; }

    [JsonPropertyName("countryOrRegionServingStateReasons")]
    public object? CountryOrRegionServingStateReasons { get; set; }

    [JsonPropertyName("countryOrRegion")]
    public string? CountryOrRegion { get; set; }

    [JsonPropertyName("deviceClass")]
    public string? DeviceClass { get; set; }
}

public class CampaignReportAppDto
{
    [JsonPropertyName("appName")]
    public string? AppName { get; set; }

    [JsonPropertyName("adamId")]
    public long? AdamId { get; set; }
}
