namespace InventoryService.Commands;

public record CreateCategoryCommand(string Name, Guid? ParentCategoryId);

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
    }
}

public static partial class DtoMapExtensions
{
    public static CategoryEntity ToNewCategoryEntity(this CreateCategoryCommand command, Guid currentUserId, Guid tenantId)
    {
        return new CategoryEntity()
        {
            Id = default,
            Name = command.Name,
            TenantId = tenantId,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = currentUserId,
            UpdatedByUserId = currentUserId,
            ParentCategoryId = command.ParentCategoryId
        };
    }
}

public class CreateCategoryCommandHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : IResourceCommandHandler<CreateCategoryCommand>
{
    public async ValueTask<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken token)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Categories.AddAsync(command.ToNewCategoryEntity(Guid.NewGuid(), Guid.NewGuid()));
            await dbContext.SaveChangesAsync();
            return entity.Entity.Id;
        }
    }
}