using Microsoft.EntityFrameworkCore;

namespace InventoryService.Commands;

public record UpdateProductCommand(Guid Id, string? Name, Guid? CategoryId, decimal? Price, int? NewStock);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(c => c.Name).MinimumLength(2).When(c => c.Name != null);
        RuleFor(c => c.Price).GreaterThan(0).When(c => c.Price != null);
        RuleFor(c => c.NewStock).GreaterThanOrEqualTo(0).When(c => c.NewStock != null);
    }
}

public class UpdateProductCommandHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
: IResourceCommandHandler<UpdateProductCommand>
{
    public async ValueTask<Guid> HandleAsync(UpdateProductCommand command, CancellationToken token)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Products
                .Include(c => c.ProductStock)
                .FirstOrDefaultAsync(c => c.Id == command.Id, token);

            if (entity == null)
                throw new Exception("NOT FOUND");

            if (command.Name != null)
                entity.Name = command.Name;

            if (command.Price != null)
                entity.Price = command.Price.Value;

            if (command.CategoryId != null)
                entity.CategoryId = command.CategoryId;

            if (command.NewStock != null)
                entity.ProductStock.Quantity = command.NewStock.Value;

            await dbContext.SaveChangesAsync();
            return entity.Id;
        }
    }
}