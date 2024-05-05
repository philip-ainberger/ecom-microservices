using System.ComponentModel.DataAnnotations;

namespace TokenService.Specification.ServiceConsumer;

public class TokenServiceEndpointSettings
{
    [Required]
    public required string TokenServiceEndpoint { get; set; }
}