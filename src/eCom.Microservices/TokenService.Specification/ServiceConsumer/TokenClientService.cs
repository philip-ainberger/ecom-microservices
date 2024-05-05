using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using TokenService.Specification.Grpc.TokenService;

namespace TokenService.Specification.ServiceConsumer;

public interface ITokenClientService
{
    Task<CreateTokenResponse> CreateTokenAsync(string userEmail, CancellationToken token);
}

public class TokenClientService(IOptions<TokenServiceEndpointSettings> options) : ITokenClientService
{
    public async Task<CreateTokenResponse> CreateTokenAsync(string userEmail, CancellationToken token)
    {
        using var channel = GrpcChannel.ForAddress(options.Value.TokenServiceEndpoint);
        var client = new TokenGrpcService.TokenGrpcServiceClient(channel);

        var reply = await client.CreateTokenAsync(new CreateTokenRequest()
        { UserEmail = userEmail }, cancellationToken: token);

        return reply;
    }
}
