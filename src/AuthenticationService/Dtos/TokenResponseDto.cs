namespace AuthenticationService.Dtos;

public record TokenResponseDto(string AccessToken, long ExpiresIn);