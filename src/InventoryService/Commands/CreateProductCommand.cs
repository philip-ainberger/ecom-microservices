﻿namespace InventoryService.Commands;

public record CreateProductCommand(string Name, decimal Price, Guid? CategoryId, int InitialStock);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
        RuleFor(c => c.Price).GreaterThan(0);
        RuleFor(c => c.InitialStock).GreaterThanOrEqualTo(0);
    }
}

public static partial class DtoMapExtensions
{
    public static ProductEntity ToNewProductEntity(this CreateProductCommand command, Guid currentUserId, Guid tenantId)
    {
        return new ProductEntity()
        {
            Id = default,
            Name = command.Name,
            TenantId = tenantId,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = currentUserId,
            UpdatedByUserId = currentUserId,
            CategoryId = command.CategoryId,
            Price = command.Price,
            ProductStock = new ProductStockEntity()
            {
                Quantity = command.InitialStock,
                ReorderLevel = -1,
                Reserved = 0,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = currentUserId,
                UpdatedByUserId = currentUserId,
                TenantId = tenantId
            }
        };
    }
}

public class CreateProductCommandHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
: IResourceCommandHandler<CreateProductCommand>
{
    public async ValueTask<Guid> HandleAsync(CreateProductCommand command, CancellationToken token)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Products.AddAsync(command.ToNewProductEntity(Guid.NewGuid(), Guid.NewGuid()));
            await dbContext.SaveChangesAsync();
            return entity.Entity.Id;
        }
    }
}