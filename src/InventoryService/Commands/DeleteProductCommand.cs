namespace InventoryService.Commands;

public record DeleteProductCommand(Guid Id);

public class DeleteProductCommandHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : ICommandHandler<DeleteProductCommand>
{
    public async ValueTask HandleAsync(DeleteProductCommand command, CancellationToken token)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Products.FindAsync(command.Id);
            if (entity == null)
                throw new Exception("Entity not found");

            dbContext.Products.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}