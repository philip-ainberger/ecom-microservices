namespace Service.Base.Api.CQRS;

public interface IQueryHandler<in T, TResult>
{
    ValueTask<TResult> HandleAsync(T query, CancellationToken ct);
}

public delegate ValueTask<TResult> QueryHandler<in T, TResult>(T query, CancellationToken ct);