using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService;

public record ProductEntity : BaseEntity
{
    [Required]
    public required string Name { get; init; }

    [Required,Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; init; }

    public Guid? CategoryId { get; init; }

    [ForeignKey("CategoryId")]
    public CategoryEntity? Category { get; init; }
}