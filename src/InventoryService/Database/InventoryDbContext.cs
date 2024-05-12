using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public DbSet<ProductStockEntity> ProductsStock { get; set; }

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

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>()
            .HasOne<CategoryEntity>()
            .WithOne(c => c.ParentCategory)
            .HasForeignKey<CategoryEntity>(c => c.ParentCategoryId);

        modelBuilder.Entity<CategoryEntity>()
            .HasMany(c => c.Products)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId);

        modelBuilder.Entity<ProductEntity>()
            .HasOne(s => s.ProductStock)
            .WithOne(p => p.Product)
            .HasForeignKey<ProductStockEntity>(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
