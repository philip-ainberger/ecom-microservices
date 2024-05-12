namespace InventoryService.Dtos;

public record ProductDto(
    Guid Id,
    Guid TenantId,
    DateTime UpdatedAt,
    DateTime CreatedAt,
    Guid CreatedByUserId,
    Guid UpdatedByUserId,
    string Name,
    decimal Price,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Guid? CategoryId);

public static partial class DtoMapExtensions
{
    public static ProductDto ToProductDto(this ProductEntity entity)
    {
        return new ProductDto(entity.Id, entity.TenantId, entity.UpdatedAt, entity.CreatedAt, entity.CreatedByUserId, entity.UpdatedByUserId, entity.Name, entity.Price, entity.CategoryId);
    }
}