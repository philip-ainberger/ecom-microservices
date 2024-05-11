using Microsoft.EntityFrameworkCore;

namespace Service.Base.EntityFrameworkCore;

public interface IDbContextProvider<T> where T : DbContext
{
    T ProvideContext();
}

public class DbContextProvider<T> : IDbContextProvider<T> where T : DbContext
{
    private readonly IDbContextFactory<T> _factory;

    public DbContextProvider(IDbContextFactory<T> factory)
    {
        _factory = factory;
    }

    public T ProvideContext()
    {
        var context = _factory.CreateDbContext();

        context.Database.EnsureCreated();

        return context;
    }
}