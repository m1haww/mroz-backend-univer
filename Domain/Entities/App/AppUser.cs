namespace Domain.Entities.App;

public class AppUser
{
    public Guid Id { get; set; }
    public string AppId { get; set; } = string.Empty;
    public double TotalRevenue { get; set; }
    public DateTime InstallDate { get; set; }
    public long? CampaignId { get; set; }
    public long? KeywordId { get; set; }
    public long? AdGroupId { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public bool HasTrial { get; set; }
}
