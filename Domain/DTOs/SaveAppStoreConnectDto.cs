namespace Domain.DTOs;

using System.ComponentModel.DataAnnotations;

public class SaveAppStoreConnectDto
{
    [Required(ErrorMessage = "Key ID is required.")]
    [MaxLength(100)]
    public string KeyId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Issuer ID is required.")]
    [MaxLength(100)]
    public string IssuerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Private key (.p8) is required.")]
    public string PrivateKey { get; set; } = string.Empty;
}

public class AppStoreConnectStatusResponse
{
    public bool Configured { get; set; }
    public string? KeyIdSuffix { get; set; }
}
