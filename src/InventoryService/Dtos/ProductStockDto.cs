namespace InventoryService.Dtos;

public record ProductStockDto(
    Guid Id,
    Guid TenantId,
    DateTime UpdatedAt,
    DateTime CreatedAt,
    Guid CreatedByUserId,
    Guid UpdatedByUserId,
    int Quantity,
    int Reserved,
    int ReorderLevel,
    Guid ProductId);