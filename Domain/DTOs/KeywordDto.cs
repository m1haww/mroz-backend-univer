using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class KeywordDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("campaignId")]
    public long CampaignId { get; set; }

    [JsonPropertyName("adGroupId")]
    public long AdGroupId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("matchType")]
    public string MatchType { get; set; } = "BROAD";

    [JsonPropertyName("bidAmount")]
    public MoneyDto? BidAmount { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime? CreationTime { get; set; }

    [JsonPropertyName("modificationTime")]
    public DateTime? ModificationTime { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}

public class KeywordListResponseDto
{
    [JsonPropertyName("data")]
    public List<KeywordDto>? Data { get; set; }
}
