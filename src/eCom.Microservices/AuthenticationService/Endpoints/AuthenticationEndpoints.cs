using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Dtos;
using AuthenticationService.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using FluentResults;
using TokenService.Specification.ServiceConsumer;
using AuthenticationService.Metrics;

namespace AuthenticationService.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/authentication/user/signup", SignUpAsync).AllowAnonymous();
        endpointRouteBuilder.MapPost("/authentication/user/login", LoginAsync).AllowAnonymous();

        endpointRouteBuilder.MapPost("/authentication/user/logout", LogoutAsync).RequireAuthorization();
        endpointRouteBuilder.MapPost("/authentication/user/refresh", RefreshApiToken).RequireAuthorization();
    }

    public static async Task<Ok<TokenResponseDto>> RefreshApiToken(HttpContext http, CancellationToken token, ITokenClientService tokenService, IAuthenticationMetrics metrics)
    {
        ClaimsPrincipal user = http.User;

        var reply = await tokenService.CreateTokenAsync(user.FindFirstValue(ClaimTypes.Email), token);

        metrics.RefreshedToken();

        return TypedResults.Ok(new TokenResponseDto(reply.AccessToken, reply.ExpiresIn));
    }

    public static async Task<Ok> LogoutAsync(HttpContext http)
    {
        await http.SignOutAsync();

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, BadRequest, UnauthorizedHttpResult, StatusCodeHttpResult>>
        LoginAsync(HttpContext http, CancellationToken token, [FromBody] LoginDto login, IUserAuthenticationService service)
    {
        var result = await service.LoginAsync(login, token);

        if (result.IsFailed)
            return HandleFailure(result);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, login.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await http.SignInAsync(claimsPrincipal);

        return TypedResults.Ok();
    }

    public static async Task<Results<Created, BadRequest, UnauthorizedHttpResult, StatusCodeHttpResult>>
        SignUpAsync(CancellationToken token, [FromBody] SignUpDto signUp, IUserAuthenticationService service)
    {
        var result = await service.SignUpAsync(signUp, token);

        if (result.IsFailed)
            return HandleFailure(result);

        return TypedResults.Created();
    }

    private static BadRequest HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ => TypedResults.BadRequest()
        };
    }
}