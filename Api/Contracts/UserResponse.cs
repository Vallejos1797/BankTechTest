namespace Api.Contracts;

public record UserResponse(
    int Id,
    string Nombre,
    string Email,
    string Rol
);