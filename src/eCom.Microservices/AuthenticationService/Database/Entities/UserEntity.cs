namespace AuthenticationService.Database.Entities;

public record UserEntity(
    int Id,
    string Email,
    string HashedPassword,
    string FirstName,
    string LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime PasswordChangedAt);