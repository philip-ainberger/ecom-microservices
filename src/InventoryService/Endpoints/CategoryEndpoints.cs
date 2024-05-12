using InventoryService.Commands;
namespace InventoryService;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", GetCategories).AllowAnonymous();
        app.MapGet("/categories/{Id}", GetCategory).AllowAnonymous();
        app.MapPost("/categories", AddCategory).AllowAnonymous();
        app.MapDelete("/categories/{Id}", DeleteCategory).AllowAnonymous();
    }
    public static async Task<IResult> GetCategory(IQueryHandler<CategoryQuery, CategoryDto> handler, [FromRoute] Guid Id, CancellationToken ct)
    {
        return Results.Ok(await handler.HandleAsync(new CategoryQuery(Id), ct));
    }

    public static async Task<IResult> GetCategories(IQueryHandler<CategoriesQuery, List<CategoryDto>> handler, CancellationToken ct)
    {
        return Results.Ok(await handler.HandleAsync(new CategoriesQuery(), ct));
    }

    public static async Task<IResult> AddCategory(IValidator<CreateCategoryCommand> validator, IResourceCommandHandler<CreateCategoryCommand> handler, [FromBody] CreateCategoryCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return Results.Ok(await handler.HandleAsync(command, ct));
    }

    public static async Task<IResult> DeleteCategory(ICommandHandler<DeleteCategoryCommand> handler, [FromRoute] Guid Id, CancellationToken ct)
    {
        await handler.HandleAsync(new DeleteCategoryCommand(Id), ct);

        return Results.Ok();
    }
}