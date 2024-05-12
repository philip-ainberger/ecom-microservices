namespace InventoryService;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public Guid CreatedByUserId { get; set; }

    [Required]
    public Guid UpdatedByUserId { get; set; }
}