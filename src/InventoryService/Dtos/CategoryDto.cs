namespace InventoryService;

public record CategoryDto(Guid Id, Guid TenantId, DateTime UpdatedAt, DateTime CreatedAt, Guid CreatedByUserId, Guid UpdatedByUserId, string Name, Guid? ParentCategoryId)
{
    public CategoryDto? ParentCategory { get; init; }
}

public static partial class DtoMapExtensions
{
    public static CategoryEntity ToNewCategoryEntity(this CategoryDto dto, Guid currentUserId, Guid tenantId)
    {
        return new CategoryEntity()
        {
            Id = default,
            Name = dto.Name,
            TenantId = tenantId,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = currentUserId,
            UpdatedByUserId = currentUserId,
            ParentCategoryId = dto.ParentCategoryId
        };
    }

    public static CategoryDto ToCategoryDto(this CategoryEntity entity) {
        return new CategoryDto(entity.Id, entity.TenantId, entity.UpdatedAt, entity.CreatedAt, entity.CreatedByUserId, entity.UpdatedByUserId, entity.Name, entity.ParentCategoryId);
    }
}