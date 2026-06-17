using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class AdGroupDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("campaignId")]
    public long CampaignId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("defaultBidAmount")]
    public MoneyDto? DefaultBidAmount { get; set; }

    [JsonPropertyName("pricingModel")]
    public string PricingModel { get; set; } = string.Empty;

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("biddingStrategy")]
    public string? BiddingStrategy { get; set; }

    [JsonPropertyName("automatedKeywordsOptIn")]
    public bool AutomatedKeywordsOptIn { get; set; }

    [JsonPropertyName("automatedKeywordsRequired")]
    public bool AutomatedKeywordsRequired { get; set; }

    [JsonPropertyName("cpaGoal")]
    public MoneyDto? CpaGoal { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("displayStatus")]
    public string? DisplayStatus { get; set; }

    [JsonPropertyName("modificationTime")]
    public DateTime? ModificationTime { get; set; }

    [JsonPropertyName("orgId")]
    public long? OrgId { get; set; }

    [JsonPropertyName("paymentModel")]
    public string? PaymentModel { get; set; }

    [JsonPropertyName("servingStateReasons")]
    public IReadOnlyList<string>? ServingStateReasons { get; set; }

    [JsonPropertyName("servingStatus")]
    public string? ServingStatus { get; set; }

    [JsonPropertyName("targetingDimensions")]
    public TargetingDimensionsDto? TargetingDimensions { get; set; }
}

public class TargetingDimensionsDto
{
    [JsonPropertyName("age")]
    public TargetingDimensionDto? Age { get; set; }

    [JsonPropertyName("gender")]
    public TargetingDimensionDto? Gender { get; set; }

    [JsonPropertyName("deviceClass")]
    public IReadOnlyList<string>? DeviceClass { get; set; }

    [JsonPropertyName("country")]
    public IReadOnlyList<string>? Country { get; set; }

    [JsonPropertyName("adminArea")]
    public IReadOnlyList<string>? AdminArea { get; set; }

    [JsonPropertyName("locality")]
    public IReadOnlyList<string>? Locality { get; set; }
}

public class TargetingDimensionDto
{
    [JsonPropertyName("included")]
    public IReadOnlyList<string>? Included { get; set; }

    [JsonPropertyName("excluded")]
    public IReadOnlyList<string>? Excluded { get; set; }
}

public class AdGroupResponseDto
{
    [JsonPropertyName("data")]
    public AdGroupDto? Data { get; set; }
}

public class AdGroupListResponseDto
{
    [JsonPropertyName("data")]
    public List<AdGroupDto>? Data { get; set; }
}

public class CreateAdGroupDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("defaultBidAmount")]
    public MoneyDto? DefaultBidAmount { get; set; }

    [JsonPropertyName("pricingModel")]
    public string PricingModel { get; set; } = "CPC";

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("automatedKeywordsOptIn")]
    public bool AutomatedKeywordsOptIn { get; set; }

    [JsonPropertyName("cpaGoal")]
    public MoneyDto? CpaGoal { get; set; }

    [JsonPropertyName("targetingDimensions")]
    public TargetingDimensionsDto? TargetingDimensions { get; set; }
}

public class UpdateAdGroupDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("defaultBidAmount")]
    public MoneyDto? DefaultBidAmount { get; set; }

    [JsonPropertyName("cpaGoal")]
    public MoneyDto? CpaGoal { get; set; }

    [JsonPropertyName("automatedKeywordsOptIn")]
    public bool? AutomatedKeywordsOptIn { get; set; }

    [JsonPropertyName("targetingDimensions")]
    public TargetingDimensionsDto? TargetingDimensions { get; set; }
}
