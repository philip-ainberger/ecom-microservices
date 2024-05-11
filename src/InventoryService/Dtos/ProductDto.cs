namespace InventoryService;

public record ProductDto(Guid Id, Guid TenantId, DateTime UpdatedAt, DateTime CreatedAt, Guid CreatedByUserId, Guid UpdatedByUserId, string Name, decimal Price, Guid? CategoryId)
{
    public CategoryDto? Category { get; init; }
}