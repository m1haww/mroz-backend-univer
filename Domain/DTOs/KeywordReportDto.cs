using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class KeywordReportRequestDto
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

    [JsonPropertyName("selector")]
    public KeywordReportSelectorDto? Selector { get; set; }
}

public class KeywordReportSelectorDto
{
    [JsonPropertyName("orderBy")]
    public List<KeywordReportOrderByDto>? OrderBy { get; set; }

    [JsonPropertyName("conditions")]
    public List<KeywordReportConditionDto>? Conditions { get; set; }

    [JsonPropertyName("pagination")]
    public KeywordReportPaginationDto? Pagination { get; set; }
}

public class KeywordReportOrderByDto
{
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("sortOrder")]
    public string? SortOrder { get; set; }
}

public class KeywordReportConditionDto
{
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("operator")]
    public string? Operator { get; set; }

    [JsonPropertyName("values")]
    public List<string>? Values { get; set; }
}

public class KeywordReportPaginationDto
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}

public class KeywordReportResponseDto
{
    [JsonPropertyName("row")]
    public List<KeywordReportRowDto>? Row { get; set; }

    [JsonPropertyName("grandTotals")]
    public KeywordReportGrandTotalsDto? GrandTotals { get; set; }
}

public class KeywordReportRowDto
{
    [JsonPropertyName("other")]
    public bool Other { get; set; }

    [JsonPropertyName("total")]
    public KeywordReportTotalDto? Total { get; set; }

    [JsonPropertyName("metadata")]
    public KeywordReportMetadataDto? Metadata { get; set; }

    [JsonPropertyName("insights")]
    public KeywordReportInsightsDto? Insights { get; set; }
}

public class KeywordReportGrandTotalsDto
{
    [JsonPropertyName("other")]
    public bool Other { get; set; }

    [JsonPropertyName("total")]
    public KeywordReportTotalDto? Total { get; set; }
}

public class KeywordReportTotalDto
{
    [JsonPropertyName("impressions")]
    public int? Impressions { get; set; }

    [JsonPropertyName("taps")]
    public int? Taps { get; set; }

    [JsonPropertyName("ttr")]
    public decimal? Ttr { get; set; }

    [JsonPropertyName("avgCPT")]
    public MoneyDto? AvgCpt { get; set; }

    [JsonPropertyName("avgCPM")]
    public MoneyDto? AvgCpm { get; set; }

    [JsonPropertyName("localSpend")]
    public MoneyDto? LocalSpend { get; set; }

    [JsonPropertyName("totalInstalls")]
    public int? TotalInstalls { get; set; }

    [JsonPropertyName("totalNewDownloads")]
    public int? TotalNewDownloads { get; set; }

    [JsonPropertyName("totalRedownloads")]
    public int? TotalRedownloads { get; set; }

    [JsonPropertyName("viewInstalls")]
    public int? ViewInstalls { get; set; }

    [JsonPropertyName("tapInstalls")]
    public int? TapInstalls { get; set; }

    [JsonPropertyName("tapNewDownloads")]
    public int? TapNewDownloads { get; set; }

    [JsonPropertyName("tapRedownloads")]
    public int? TapRedownloads { get; set; }

    [JsonPropertyName("viewNewDownloads")]
    public int? ViewNewDownloads { get; set; }

    [JsonPropertyName("viewReDownloads")]
    public int? ViewReDownloads { get; set; }

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

    [JsonPropertyName("tapInstallCPI")]
    public MoneyDto? TapInstallCpi { get; set; }

    [JsonPropertyName("tapInstallRate")]
    public decimal? TapInstallRate { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }
}

public class KeywordReportMetadataDto
{
    [JsonPropertyName("keywordId")]
    public long? KeywordId { get; set; }

    [JsonPropertyName("keyword")]
    public string? Keyword { get; set; }

    [JsonPropertyName("keywordStatus")]
    public string? KeywordStatus { get; set; }

    [JsonPropertyName("matchType")]
    public string? MatchType { get; set; }

    [JsonPropertyName("bidAmount")]
    public MoneyDto? BidAmount { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("keywordDisplayStatus")]
    public string? KeywordDisplayStatus { get; set; }

    [JsonPropertyName("adGroupId")]
    public long? AdGroupId { get; set; }

    [JsonPropertyName("adGroupName")]
    public string? AdGroupName { get; set; }

    [JsonPropertyName("adGroupDeleted")]
    public bool AdGroupDeleted { get; set; }

    [JsonPropertyName("modificationTime")]
    public string? ModificationTime { get; set; }
}

public class KeywordReportInsightsDto
{
    [JsonPropertyName("bidRecommendation")]
    public KeywordReportBidRecommendationDto? BidRecommendation { get; set; }
}

public class KeywordReportBidRecommendationDto
{
    [JsonPropertyName("bidMin")]
    public MoneyDto? BidMin { get; set; }

    [JsonPropertyName("bidMax")]
    public MoneyDto? BidMax { get; set; }

    [JsonPropertyName("suggestedBidAmount")]
    public MoneyDto? SuggestedBidAmount { get; set; }
}
