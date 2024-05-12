namespace InventoryService.Queries;

public record ProductQuery(Guid Id);

public class ProductQueryHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : IQueryHandler<ProductQuery, ProductDto>
{
    public async ValueTask<ProductDto> HandleAsync(ProductQuery query, CancellationToken ct)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Products.FindAsync(query.Id);

            if (entity == null)
                throw new Exception("NOT FOUND");

            return entity.ToProductDto();
        }
    }
}