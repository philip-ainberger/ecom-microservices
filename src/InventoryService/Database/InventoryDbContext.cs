using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Service.Base.EntityFrameworkCore.Settings;

namespace InventoryService;

public class InventoryDbContext : DbContext
{
    private readonly IOptions<EntityFrameworkSettings> _options;

    public InventoryDbContext(IOptions<EntityFrameworkSettings> options)
    {
        _options = options;
    }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductStockEntity> ProductsStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = _options.Value.Host,
            Port = _options.Value.Port,
            Database = _options.Value.Database,
            Username = _options.Value.Username,
            Password = _options.Value.Password
        };

        var con = npgsqlConnectionStringBuilder.ConnectionString;

        optionsBuilder.UseNpgsql(con);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
