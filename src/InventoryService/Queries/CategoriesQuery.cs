namespace InventoryService.Queries;

public record CategoriesQuery();

public class CategoriesQueryHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : IQueryHandler<CategoriesQuery, List<CategoryDto>>
{
    public async ValueTask<List<CategoryDto>> HandleAsync(CategoriesQuery query, CancellationToken ct)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            return dbContext.Categories
                .AsQueryable()
                .Select(c => c.ToCategoryDto())
                .ToList();
        }
    }
}