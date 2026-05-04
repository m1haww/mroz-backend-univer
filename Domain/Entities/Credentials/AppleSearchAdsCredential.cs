namespace Domain.Entities.Credentials;

public class AppleSearchAdsCredential
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User.User User { get; set; } = null!;
    public string PrivateKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string TeamId { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
    public string Alg { get; set; } = "ES256";
    public string? ClientSecretJwt { get; set; }
    public DateTime? ClientSecretJwtExpiresAt { get; set; }
    public int? OrgId { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
