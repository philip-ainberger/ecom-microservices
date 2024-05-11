using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService;

public record class ProductStockEntity : BaseEntity
{
    // maybe add warehouse support later

    [Required]
    public int Quantity { get; init; }
    
    [Required]
    public int Reserved { get; init; }
    
    [Required]
    public int ReorderLevel { get; init; }

    [Required]
    public Guid ProductId { get; init; }

    [ForeignKey("ProductId")]
    public required ProductEntity Product { get; init; }
}
