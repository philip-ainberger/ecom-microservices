namespace InventoryService;

public class CategoryEntity : BaseEntity
{
    [Required]
    public required string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }

    [ForeignKey("ParentCategoryId")]
    public CategoryEntity? ParentCategory { get; set; }

    public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
