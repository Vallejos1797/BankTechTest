namespace Api.Contracts;

public record AuthResponse(
    string Token,
    UserResponse Usuario
);