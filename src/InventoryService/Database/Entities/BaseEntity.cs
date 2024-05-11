using System.ComponentModel.DataAnnotations;

namespace InventoryService;

public record BaseEntity
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public Guid TenantId { get; init; }

    [Required]
    public DateTime UpdatedAt { get; init; }
    
    [Required]
    public DateTime CreatedAt { get; init; }
    
    [Required]
    public Guid CreatedByUserId { get; init; }
    
    [Required]
    public Guid UpdatedByUserId { get; init; }
}