namespace InventoryService.Queries;

public record CategoryQuery(Guid Id);

public class CategoryQueryHandler(IDbContextProvider<InventoryDbContext> dbContextProvider)
    : IQueryHandler<CategoryQuery, CategoryDto>
{
    public async ValueTask<CategoryDto> HandleAsync(CategoryQuery query, CancellationToken ct)
    {
        using (var dbContext = dbContextProvider.ProvideContext())
        {
            var entity = await dbContext.Categories.FindAsync(query.Id);

            if (entity == null)
                throw new Exception("NOT FOUND");

            return entity.ToCategoryDto();
        }
    }
}