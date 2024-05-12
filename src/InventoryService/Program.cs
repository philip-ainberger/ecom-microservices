using InventoryService;
using InventoryService.Endpoints;
using Service.Base.EntityFrameworkCore.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkSettings();

builder.Services.AddDbContextProvider<InventoryDbContext>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddCategoryCqrs();
builder.Services.AddProductCqrs();

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