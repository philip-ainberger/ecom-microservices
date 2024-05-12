namespace InventoryService.Queries;

public record ProductsQuery();

public class ProductsQueryHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : IQueryHandler<ProductsQuery, List<ProductDto>>
{
    public async ValueTask<List<ProductDto>> HandleAsync(ProductsQuery query, CancellationToken ct)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            return dbContext.Products
                .AsQueryable()
                .Select(DtoMapExtensions.ToProductDto)
                .ToList();
        }
    }
}