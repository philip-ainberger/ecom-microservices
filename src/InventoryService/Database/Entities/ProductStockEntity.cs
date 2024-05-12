namespace InventoryService;

public class ProductStockEntity : BaseEntity
{
    // maybe add warehouse support later

    [Required]
    public int Quantity { get; set; }

    [Required]
    public int Reserved { get; set; }

    [Required]
    public int ReorderLevel { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;
}
