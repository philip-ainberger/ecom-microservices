using AuthenticationService.Database.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Dtos;

public record SignUpDto(string Email, string Password, string FirstName, string LastName);

public static partial class DtoMapExtensions
{
    public static UserEntity ToUserEntity(this SignUpDto dto)
    {
        var hasher = new PasswordHasher<UserEntity>();
        var hashedPassword = hasher.HashPassword(default, dto.Password);

        return new UserEntity(default, dto.Email.ToLower(), hashedPassword, dto.FirstName, dto.LastName, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
    }
}

public class SignUpValidator : AbstractValidator<SignUpDto>
{
    public SignUpValidator()
    {
        RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotNull().NotEmpty().MinimumLength(Constants.PasswordMinLength);
        RuleFor(c => c.FirstName).NotNull().NotEmpty();
        RuleFor(c => c.LastName).NotNull().NotEmpty();
    }
}