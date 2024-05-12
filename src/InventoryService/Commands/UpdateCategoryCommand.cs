using Microsoft.EntityFrameworkCore;

namespace InventoryService.Commands;

public record UpdateCategoryCommand(Guid Id, string? Name, Guid? ParentCategoryId);

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).MinimumLength(2).When(c => c.Name != null);
    }
}

public class UpdateCategoryCommandHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : IResourceCommandHandler<UpdateCategoryCommand>
{
    public async ValueTask<Guid> HandleAsync(UpdateCategoryCommand command, CancellationToken token)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == command.Id, token);

            if (entity == null)
                throw new Exception("NOT FOUND");

            if (command.Name != null)
                entity.Name = command.Name;

            if (command.ParentCategoryId != null)
                entity.ParentCategoryId = command.ParentCategoryId.Value;

            await dbContext.SaveChangesAsync();
            return entity.Id;
        }
    }
}