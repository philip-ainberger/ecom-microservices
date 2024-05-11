using Microsoft.EntityFrameworkCore;

namespace InventoryService;

public class InventoryService(InventoryDbContext dbContext)
{

    public async Task<Guid> AddCategoryAsync(CategoryDto category)
    {
        var entity = await dbContext.Categories.AddAsync(category.ToNewCategoryEntity(Guid.NewGuid(),Guid.NewGuid()));
        await dbContext.SaveChangesAsync();
        return entity.Entity.Id;
    }

}
