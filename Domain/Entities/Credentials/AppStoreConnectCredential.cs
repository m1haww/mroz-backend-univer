namespace Domain.Entities.Credentials;

public class AppStoreConnectCredential
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User.User User { get; set; } = null!;

    public string IssuerId { get; set; } = string.Empty;

    public string KeyId { get; set; } = string.Empty;

    public string PrivateKey { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
