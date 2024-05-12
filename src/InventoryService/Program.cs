using InventoryService;
using InventoryService.Commands;
using InventoryService.Endpoints;
using Service.Base.Api.Setup;
using Service.Base.EntityFrameworkCore.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkSettings();

builder.Services.AddDbContextProvider<InventoryDbContext>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddResourceCommandHandler<CreateCategoryCommand, CreateCategoryCommandHandler>();
builder.Services.AddCommandHandler<DeleteCategoryCommand, DeleteCategoryCommandHandler>();

builder.Services.AddQueryHandler<CategoriesQuery, List<CategoryDto>, CategoriesQueryHandler>();
builder.Services.AddQueryHandler<CategoryQuery, CategoryDto, CategoryQueryHandler>();

builder.Services.AddResourceCommandHandler<CreateProductCommand, CreateProductCommandHandler>();
builder.Services.AddCommandHandler<DeleteProductCommand, DeleteProductCommandHandler>();

builder.Services.AddQueryHandler<ProductsQuery, List<ProductDto>, ProductsQueryHandler>();
builder.Services.AddQueryHandler<ProductQuery, ProductDto, ProductQueryHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapCategoryEndpoints();
app.MapProductEndpoints();

app.Run();

partial class Program
{
    protected Program() { }
}