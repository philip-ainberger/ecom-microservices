using Grpc.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenService.Settings;
using TokenService.Specification.Grpc.TokenService;

namespace TokenService.Grpc;

public class TokenGrpcService(IOptions<JwtSettings> options, ILogger<TokenGrpcService> logger) : Specification.Grpc.TokenService.TokenGrpcService.TokenGrpcServiceBase
{
    private byte[] SymmetricSecurityKey => Encoding.UTF8.GetBytes(options.Value.Secret);

    public override async Task<ValidateTokenResponse> ValidateToken(ValidateTokenRequest request, ServerCallContext context)
    {
        logger.LogDebug("[{RequestType}] Received request ...", request.GetType());

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            IssuerSigningKey = new SymmetricSecurityKey(SymmetricSecurityKey),
            ValidIssuer = options.Value.Issuer,
            ValidAudience = options.Value.Audience,
        };

        var validationResult = await jwtTokenHandler.ValidateTokenAsync(request.AccessToken, validationParams);

        if (validationResult.IsValid)
        {
            logger.LogDebug("Token is valid");
            return new ValidateTokenResponse() { IsValid = true };
        }
        else
        {
            logger.LogInformation("Token is invalid!");
            return new ValidateTokenResponse() { IsValid = false };
        }
    }

    public override Task<CreateTokenResponse> CreateToken(CreateTokenRequest request, ServerCallContext context)
    {
        logger.LogDebug("[{RequestType}] Received request ...", request.GetType());

        var signingKey = new SymmetricSecurityKey(SymmetricSecurityKey);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claim = new Claim(JwtRegisteredClaimNames.Sub, request.UserEmail);
        var expiration = DateTime.UtcNow.AddMinutes(options.Value.TokenExpirationInMinutes);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>() { claim }),
            Expires = expiration,
            SigningCredentials = credentials,
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtTokenHandler.CreateToken(securityTokenDescriptor);
        var tokenString = jwtTokenHandler.WriteToken(securityToken);

        logger.LogDebug("Token created successfully");

        return Task.FromResult(new CreateTokenResponse()
        {
            AccessToken = tokenString,
            ExpiresIn = expiration.Ticks
        });
    }
}