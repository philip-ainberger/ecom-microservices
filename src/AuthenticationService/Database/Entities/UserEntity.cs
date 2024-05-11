namespace AuthenticationService.Database.Entities;

public record UserEntity(
    Guid Id,
    string Email,
    string HashedPassword,
    string FirstName,
    string LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime PasswordChangedAt);