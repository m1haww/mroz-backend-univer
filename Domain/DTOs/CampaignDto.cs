using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class MoneyDto
{
    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}

public class LocInvoiceDetailsDto
{
    [JsonPropertyName("invoiceId")]
    public string? InvoiceId { get; set; }

    [JsonPropertyName("invoiceDate")]
    public DateTime? InvoiceDate { get; set; }

    [JsonPropertyName("invoiceNumber")]
    public string? InvoiceNumber { get; set; }
}

public class CampaignDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("adamId")]
    public long AdamId { get; set; }

    [JsonPropertyName("adChannelType")]
    public string AdChannelType { get; set; } = string.Empty;

    [JsonPropertyName("billingEvent")]
    public string BillingEvent { get; set; } = string.Empty;

    [JsonPropertyName("biddingStrategy")]
    public string? BiddingStrategy { get; set; }

    [JsonPropertyName("budgetAmount")]
    public MoneyDto? BudgetAmount { get; set; }

    [JsonPropertyName("budgetOrders")]
    public IReadOnlyList<long>? BudgetOrders { get; set; }

    [JsonPropertyName("countriesOrRegions")]
    public IReadOnlyList<string> CountriesOrRegions { get; set; } = new List<string>();

    [JsonPropertyName("countryOrRegionServingStateReasons")]
    public IReadOnlyDictionary<string, IReadOnlyList<string>>? CountryOrRegionServingStateReasons { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime? CreationTime { get; set; }

    [JsonPropertyName("dailyBudgetAmount")]
    public MoneyDto? DailyBudgetAmount { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("displayStatus")]
    public string? DisplayStatus { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("locInvoiceDetails")]
    public LocInvoiceDetailsDto? LocInvoiceDetails { get; set; }

    [JsonPropertyName("modificationTime")]
    public DateTime? ModificationTime { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("orgId")]
    public long? OrgId { get; set; }

    [JsonPropertyName("paymentModel")]
    public string? PaymentModel { get; set; }

    [JsonPropertyName("servingStateReasons")]
    public IReadOnlyList<string>? ServingStateReasons { get; set; }

    [JsonPropertyName("servingStatus")]
    public string? ServingStatus { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("supplySources")]
    public IReadOnlyList<string> SupplySources { get; set; } = new List<string>();

    [JsonPropertyName("targetCpa")]
    public MoneyDto? TargetCpa { get; set; }
}

public class CampaignResponseDto
{
    [JsonPropertyName("data")]
    public CampaignDto? Data { get; set; }
}

public class CampaignListResponseDto
{
    [JsonPropertyName("data")]
    public List<CampaignDto>? Data { get; set; }
}

public class CreateCampaignDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("adamId")]
    public long AdamId { get; set; }

    [JsonPropertyName("adChannelType")]
    public string AdChannelType { get; set; } = "SEARCH";

    [JsonPropertyName("billingEvent")]
    public string BillingEvent { get; set; } = "TAPS";

    [JsonPropertyName("countriesOrRegions")]
    public List<string> CountriesOrRegions { get; set; } = new();

    [JsonPropertyName("dailyBudgetAmount")]
    public MoneyDto? DailyBudgetAmount { get; set; }

    [JsonPropertyName("supplySources")]
    public List<string> SupplySources { get; set; } = new();

    [JsonPropertyName("budgetAmount")]
    public MoneyDto? BudgetAmount { get; set; }

    [JsonPropertyName("biddingStrategy")]
    public string? BiddingStrategy { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("targetCpa")]
    public MoneyDto? TargetCpa { get; set; }
}

public class UpdateCampaignDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("dailyBudgetAmount")]
    public MoneyDto? DailyBudgetAmount { get; set; }

    [JsonPropertyName("budgetAmount")]
    public MoneyDto? BudgetAmount { get; set; }

    [JsonPropertyName("countriesOrRegions")]
    public List<string>? CountriesOrRegions { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("biddingStrategy")]
    public string? BiddingStrategy { get; set; }

    [JsonPropertyName("targetCpa")]
    public MoneyDto? TargetCpa { get; set; }
}
