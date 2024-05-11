using System.ComponentModel.DataAnnotations;

namespace TokenService.Settings;

public class JwtSettings
{
    [Required]
    public required string Issuer { get; set; }
    
    [Required]
    public required string Audience { get; set; }

    [Required]
    public required string Secret { get; set; }

    public int TokenExpirationInMinutes { get; set; } = 10;
}