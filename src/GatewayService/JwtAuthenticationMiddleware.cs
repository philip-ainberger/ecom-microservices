using TokenService.Specification.ServiceConsumer;

namespace GatewayService;

public class JwtAuthenticationMiddleware(ILogger<JwtAuthenticationMiddleware> logger, RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context, ITokenClientService tokenService)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization token is required");
            return;
        }

        var isValid = await ValidateTokenAsync(tokenService, token, context.RequestAborted);
        if (!isValid)
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        await next(context);
    }

    private async Task<bool> ValidateTokenAsync(ITokenClientService tokenService, string token, CancellationToken cancellationToken)
    {
        try
        {
            var reply = await tokenService.ValidateTokenAsync(token, cancellationToken);
            return reply;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception occurred");
            return false;
        }
    }
}