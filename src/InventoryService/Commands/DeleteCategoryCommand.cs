using Microsoft.EntityFrameworkCore;

namespace InventoryService.Commands;

public record DeleteCategoryCommand(Guid Id);

public class DeleteCategoryCommandHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async ValueTask HandleAsync(DeleteCategoryCommand command, CancellationToken token)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Categories.FindAsync(command.Id);
            if (entity == null)
                throw new Exception("Entity not found");

            var categoryInUse = await dbContext.Categories
                .AsQueryable()
                .AnyAsync(c => c.ParentCategoryId == entity.Id);

            if (categoryInUse)
            {
                throw new Exception("Entity not found");
            }

            dbContext.Categories.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}