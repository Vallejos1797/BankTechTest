namespace Api.Contracts;

public record RegisterRequest(string Username, string Email, string Password, string Role = "user");
public record LoginRequest(string Username, string Password);
public record UserResponse(int Id, string Username, string Email, string Role);
public record AuthResponse(string Token, UserResponse User);