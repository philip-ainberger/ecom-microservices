

namespace InventoryService.Dtos;

public record CategoryDto(
    Guid Id,
    Guid TenantId,
    DateTime UpdatedAt,
    DateTime CreatedAt,
    Guid CreatedByUserId,
    Guid UpdatedByUserId,
    string Name,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Guid? ParentCategoryId);

public static partial class DtoMapExtensions
{
    public static CategoryDto ToCategoryDto(this CategoryEntity entity)
    {
        return new CategoryDto(entity.Id, entity.TenantId, entity.UpdatedAt, entity.CreatedAt, entity.CreatedByUserId, entity.UpdatedByUserId, entity.Name, entity.ParentCategoryId);
    }
}