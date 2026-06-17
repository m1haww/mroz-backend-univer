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
    [JsonPropertyName("data")]
    public CampaignReportDataDto? Data { get; set; }
}

public class CampaignReportDataDto
{
    [JsonPropertyName("reportingDataResponse")]
    public CampaignReportingDataResponseDto? ReportingDataResponse { get; set; }
}

public class CampaignReportingDataResponseDto
{
    [JsonPropertyName("row")]
    public List<CampaignReportRowDto>? Row { get; set; }
}

public class CampaignReportRowDto
{
    [JsonPropertyName("other")]
    public bool Other { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("metadata")]
    public CampaignReportMetadataDto? Metadata { get; set; }

    [JsonPropertyName("granularity")]
    public List<CampaignReportRowTotalDto>? Granularity { get; set; }

    [JsonPropertyName("total")]
    public CampaignReportRowTotalDto? Total { get; set; }

    [JsonPropertyName("revenue")]
    public decimal? Revenue { get; set; }

    [JsonPropertyName("roas")]
    public decimal? Roas { get; set; }

    [JsonPropertyName("arpu")]
    public decimal? Arpu { get; set; }

    [JsonPropertyName("trialsCount")]
    public int? TrialsCount { get; set; }

    [JsonPropertyName("cac")]
    public decimal? Cac { get; set; }

    [JsonPropertyName("trial2PaidConversionRate")]
    public double Trial2PaidConversionRate { get; set; }

    [JsonPropertyName("install2TrialConversionRate")]
    public double Install2TrialConversionRate { get; set; }

    [JsonPropertyName("install2PaidConversionRate")]
    public double Install2PaidConversionRate { get; set; }

    [JsonPropertyName("costPerTrial")]
    public decimal? CostPerTrial { get; set; }
}

public class CampaignReportMetadataDto
{
    [JsonPropertyName("campaignId")]
    public long? CampaignId { get; set; }

    [JsonPropertyName("orgId")]
    public long? OrgId { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("modificationTime")]
    public string? ModificationTime { get; set; }

    [JsonPropertyName("campaignName")]
    public string? CampaignName { get; set; }

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

    [JsonPropertyName("totalBudget")]
    public MoneyDto? TotalBudget { get; set; }

    [JsonPropertyName("dailyBudget")]
    public MoneyDto? DailyBudget { get; set; }

    [JsonPropertyName("displayStatus")]
    public string? DisplayStatus { get; set; }

    [JsonPropertyName("supplySources")]
    public List<string>? SupplySources { get; set; }

    [JsonPropertyName("adChannelType")]
    public string? AdChannelType { get; set; }

    [JsonPropertyName("countryOrRegionServingStateReasons")]
    public Dictionary<string, object>? CountryOrRegionServingStateReasons { get; set; }

    [JsonPropertyName("billingEvent")]
    public string? BillingEvent { get; set; }

    [JsonPropertyName("biddingStrategy")]
    public string? BiddingStrategy { get; set; }

    [JsonPropertyName("targetCpa")]
    public MoneyDto? TargetCpa { get; set; }
}

public class CampaignReportAppDto
{
    [JsonPropertyName("appName")]
    public string? AppName { get; set; }

    [JsonPropertyName("adamId")]
    public long? AdamId { get; set; }
}

public class CampaignReportRowTotalDto
{
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("localSpend")]
    public MoneyDto? LocalSpend { get; set; }

    [JsonPropertyName("tapInstalls")]
    public int? TapInstalls { get; set; }

    [JsonPropertyName("tapInstallCPI")]
    public MoneyDto? TapInstallCpi { get; set; }

    [JsonPropertyName("impressions")]
    public int? Impressions { get; set; }

    [JsonPropertyName("taps")]
    public int? Taps { get; set; }

    [JsonPropertyName("ttr")]
    public decimal? Ttr { get; set; }

    [JsonPropertyName("avgCPT")]
    public MoneyDto? AvgCpt { get; set; }

    [JsonPropertyName("totalNewDownloads")]
    public int? TotalNewDownloads { get; set; }

    [JsonPropertyName("totalRedownloads")]
    public int? TotalRedownloads { get; set; }

    [JsonPropertyName("viewInstalls")]
    public int? ViewInstalls { get; set; }

    [JsonPropertyName("totalInstalls")]
    public int? TotalInstalls { get; set; }

    [JsonPropertyName("tapNewDownloads")]
    public int? TapNewDownloads { get; set; }

    [JsonPropertyName("tapRedownloads")]
    public int? TapRedownloads { get; set; }

    [JsonPropertyName("viewNewDownloads")]
    public int? ViewNewDownloads { get; set; }

    [JsonPropertyName("viewRedownloads")]
    public int? ViewRedownloads { get; set; }

    [JsonPropertyName("tapPreOrdersPlaced")]
    public int? TapPreOrdersPlaced { get; set; }

    [JsonPropertyName("viewPreOrdersPlaced")]
    public int? ViewPreOrdersPlaced { get; set; }

    [JsonPropertyName("totalPreOrdersPlaced")]
    public int? TotalPreOrdersPlaced { get; set; }

    [JsonPropertyName("totalAvgCPI")]
    public MoneyDto? TotalAvgCpi { get; set; }

    [JsonPropertyName("totalInstallRate")]
    public decimal? TotalInstallRate { get; set; }

    [JsonPropertyName("tapInstallRate")]
    public decimal? TapInstallRate { get; set; }

    [JsonPropertyName("avgCPM")]
    public MoneyDto? AvgCpm { get; set; }
}
