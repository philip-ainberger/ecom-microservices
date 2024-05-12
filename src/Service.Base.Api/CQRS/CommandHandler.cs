namespace Service.Base.Api.CQRS;

public interface ICommandHandler<in T>
{
    ValueTask HandleAsync(T command, CancellationToken token);
}

public delegate ValueTask CommandHandler<in T>(T command, CancellationToken ct);


public interface IResourceCommandHandler<in T>

{
    ValueTask<Guid> HandleAsync(T command, CancellationToken token);
}

public delegate ValueTask<Guid> ResourceCommandHandler<in T>(T command, CancellationToken ct);