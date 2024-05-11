using AuthenticationService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Service.Base.EntityFrameworkCore.Settings;

namespace AuthenticationService.Database;

public class AuthDbContext : DbContext
{
    private readonly IOptions<EntityFrameworkSettings> _options;

    public AuthDbContext(IOptions<EntityFrameworkSettings> options)
    {
        _options = options;
    }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder();
        npgsqlConnectionStringBuilder.Host = _options.Value.Host;
        npgsqlConnectionStringBuilder.Port = _options.Value.Port;
        npgsqlConnectionStringBuilder.Database = _options.Value.Database;
        npgsqlConnectionStringBuilder.Username = _options.Value.Username;
        npgsqlConnectionStringBuilder.Password = _options.Value.Password;

        var con = npgsqlConnectionStringBuilder.ConnectionString;

        optionsBuilder.UseNpgsql(con);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(c => c.HasKey(e => e.Id));

        base.OnModelCreating(modelBuilder);
    }
}