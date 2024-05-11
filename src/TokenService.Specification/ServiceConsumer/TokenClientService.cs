using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using TokenService.Specification.Grpc.TokenService;

namespace TokenService.Specification.ServiceConsumer;

public interface ITokenClientService
{
    Task<CreateTokenResponse> CreateTokenAsync(string userEmail, CancellationToken token);
    Task<bool> ValidateTokenAsync(string accessToken, CancellationToken token);
}

public class TokenClientService(IOptions<TokenServiceEndpointSettings> options) : ITokenClientService
{
    public async Task<CreateTokenResponse> CreateTokenAsync(string userEmail, CancellationToken token)
    {
        using var channel = GetTlsChannel();
        var client = new TokenGrpcService.TokenGrpcServiceClient(channel);

        var reply = await client.CreateTokenAsync(new CreateTokenRequest()
        { UserEmail = userEmail }, cancellationToken: token);

        return reply;
    }

    public async Task<bool> ValidateTokenAsync(string accessToken, CancellationToken token)
    {
        using var channel = GetTlsChannel();
        var client = new TokenGrpcService.TokenGrpcServiceClient(channel);

        var reply = await client.ValidateTokenAsync(new ValidateTokenRequest()
        { AccessToken = accessToken }, cancellationToken: token);

        return reply.IsValid;
    }

    private GrpcChannel GetTlsChannel()
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.SslProtocols = SslProtocols.Tls12;
        handler.ClientCertificates.Add(new X509Certificate2(options.Value.ClientCertificatePath, options.Value.ClientCertificatePassword));
        
        var channelOptions = new GrpcChannelOptions() { HttpHandler = handler };

        return GrpcChannel.ForAddress(options.Value.TokenServiceEndpoint, channelOptions);
    }
}