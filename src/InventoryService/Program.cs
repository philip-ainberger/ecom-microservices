using InventoryService;
using Service.Base.EntityFrameworkCore.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkSettings();

builder.Services.AddDbContextProvider<InventoryDbContext>();
builder.Services.AddScoped<InventoryService.InventoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapCategoryEndpoints();   

app.Run();