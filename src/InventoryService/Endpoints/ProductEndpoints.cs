namespace InventoryService.Endpoints;

public static class ProductEndpoints
{
    public static void AddProductCqrs(this IServiceCollection services)
    {
        services.AddResourceCommandHandler<CreateProductCommand, CreateProductCommandHandler>();
        services.AddResourceCommandHandler<UpdateProductCommand, UpdateProductCommandHandler>();
        services.AddCommandHandler<DeleteProductCommand, DeleteProductCommandHandler>();

        services.AddQueryHandler<ProductsQuery, List<ProductDto>, ProductsQueryHandler>();
        services.AddQueryHandler<ProductQuery, ProductDto, ProductQueryHandler>();
    }

    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", GetProductsAsync).AllowAnonymous();
        app.MapGet("/products/{Id}", GetProductAsync).AllowAnonymous();
        app.MapPost("/products", AddProductAsync).AllowAnonymous();
        app.MapPatch("/products/{Id}", UpdateProductAsync).AllowAnonymous();
        app.MapDelete("/products/{Id}", DeleteProductAsync).AllowAnonymous();
    }

    public static async Task<IResult> GetProductAsync(IQueryHandler<ProductQuery, ProductDto> handler, [FromRoute] Guid Id, CancellationToken ct)
    {
        return Results.Ok(await handler.HandleAsync(new ProductQuery(Id), ct));
    }

    public static async Task<IResult> GetProductsAsync(IQueryHandler<ProductsQuery, List<ProductDto>> handler, CancellationToken ct)
    {
        return Results.Ok(await handler.HandleAsync(new ProductsQuery(), ct));
    }

    public static async Task<IResult> UpdateProductAsync(
        IValidator<UpdateProductCommand> validator,
        IResourceCommandHandler<UpdateProductCommand> handler,
        [FromBody] UpdateProductCommand command,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        command = command with { Id = id };

        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return Results.Ok(await handler.HandleAsync(command, ct));
    }

    public static async Task<IResult> AddProductAsync(
        IValidator<CreateProductCommand> validator,
        IResourceCommandHandler<CreateProductCommand> handler,
        [FromBody] CreateProductCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return Results.Ok(await handler.HandleAsync(command, ct));
    }

    public static async Task<IResult> DeleteProductAsync(ICommandHandler<DeleteProductCommand> handler, [FromRoute] Guid Id, CancellationToken ct)
    {
        await handler.HandleAsync(new DeleteProductCommand(Id), ct);

        return Results.Ok();
    }
}