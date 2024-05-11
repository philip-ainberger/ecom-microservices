using FluentValidation;

namespace AuthenticationService.Dtos;

public record LoginDto(string Email, string Password);

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotNull().NotEmpty().MinimumLength(Constants.PasswordMinLength);
    }
}