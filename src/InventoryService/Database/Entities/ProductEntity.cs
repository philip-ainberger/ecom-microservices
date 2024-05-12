namespace InventoryService;

public record ProductEntity : BaseEntity
{
    [Required]
    public required string Name { get; init; }

    [Required, Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; init; }

    public Guid ProductStockId { get; init; }

    public ProductStockEntity ProductStock { get; init; } = null!;

    public Guid? CategoryId { get; init; }

    public CategoryEntity? Category { get; init; }
}