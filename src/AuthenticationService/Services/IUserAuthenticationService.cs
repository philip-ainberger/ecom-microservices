using AuthenticationService.Dtos;
using FluentResults;

namespace AuthenticationService.Services;

public interface IUserAuthenticationService
{
    Task<Result> SignUpAsync(SignUpDto signUp, CancellationToken cancellationToken);
    Task<Result> LoginAsync(LoginDto login, CancellationToken cancellationToken);
}