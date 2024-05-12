﻿namespace InventoryService;

public record CategoryEntity : BaseEntity
{
    [Required]
    public required string Name { get; init; }

    public Guid? ParentCategoryId { get; init; }

    [ForeignKey("ParentCategoryId")]
    public CategoryEntity? ParentCategory { get; init; }
}
