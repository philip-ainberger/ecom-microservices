using Microsoft.AspNetCore.Mvc;

namespace InventoryService;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapPost("/categories", AddCategory).AllowAnonymous();
    }

    public static async Task<IResult> AddCategory(InventoryService inventoryService, [FromBody] CategoryDto body)
    {
        return Results.Ok(await inventoryService.AddCategoryAsync(body));
    }
}