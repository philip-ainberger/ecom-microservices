namespace InventoryService;

public class ProductEntity : BaseEntity
{
    [Required]
    public required string Name { get; set; }

    [Required, Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    public ProductStockEntity ProductStock { get; set; } = null!;

    public Guid? CategoryId { get; set; }

    public CategoryEntity? Category { get; set; }
}