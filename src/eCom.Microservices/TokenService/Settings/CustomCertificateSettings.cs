using System.ComponentModel.DataAnnotations;

namespace TokenService.Settings;

public class CustomCertificateSettings
{
    [Required]
    public required string CertificatePath { get; set; }

    [Required]
    public required string CertificatePassword { get; set; }
}