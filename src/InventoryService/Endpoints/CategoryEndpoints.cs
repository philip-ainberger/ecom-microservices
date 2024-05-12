namespace InventoryService;

public static class CategoryEndpoints
{
    public static void AddCategoryCqrs(this IServiceCollection services)
    {
        services.AddResourceCommandHandler<CreateCategoryCommand, CreateCategoryCommandHandler>();
        services.AddResourceCommandHandler<UpdateCategoryCommand, UpdateCategoryCommandHandler>();
        services.AddCommandHandler<DeleteCategoryCommand, DeleteCategoryCommandHandler>();

        services.AddQueryHandler<CategoriesQuery, List<CategoryDto>, CategoriesQueryHandler>();
        services.AddQueryHandler<CategoryQuery, CategoryDto, CategoryQueryHandler>();
    }

    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", GetCategoriesAsync).AllowAnonymous();
        app.MapGet("/categories/{Id}", GetCategoryAsync).AllowAnonymous();
        app.MapPost("/categories", AddCategoryAsync).AllowAnonymous();
        app.MapPatch("/categories/{Id}", UpdateCategoryAsync).AllowAnonymous();
        app.MapDelete("/categories/{Id}", DeleteCategoryAsync).AllowAnonymous();
    }

    public static async Task<IResult> GetCategoryAsync(IQueryHandler<CategoryQuery, CategoryDto> handler, [FromRoute] Guid Id, CancellationToken ct)
    {
        return Results.Ok(await handler.HandleAsync(new CategoryQuery(Id), ct));
    }

    public static async Task<IResult> GetCategoriesAsync(IQueryHandler<CategoriesQuery, List<CategoryDto>> handler, CancellationToken ct)
    {
        return Results.Ok(await handler.HandleAsync(new CategoriesQuery(), ct));
    }

    public static async Task<IResult> UpdateCategoryAsync(
        IValidator<UpdateCategoryCommand> validator,
        IResourceCommandHandler<UpdateCategoryCommand> handler,
        [FromBody] UpdateCategoryCommand command,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        command = command with { Id = id };

        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return Results.Ok(await handler.HandleAsync(command, ct));
    }

    public static async Task<IResult> AddCategoryAsync(IValidator<CreateCategoryCommand> validator, IResourceCommandHandler<CreateCategoryCommand> handler, [FromBody] CreateCategoryCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return Results.Ok(await handler.HandleAsync(command, ct));
    }

    public static async Task<IResult> DeleteCategoryAsync(ICommandHandler<DeleteCategoryCommand> handler, [FromRoute] Guid Id, CancellationToken ct)
    {
        await handler.HandleAsync(new DeleteCategoryCommand(Id), ct);

        return Results.Ok();
    }
}