using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record RegisterRequest(
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    string Nombre,

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    string Email,

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
    string Password,

    [StringLength(20, ErrorMessage = "El rol no puede exceder los 20 caracteres.")]
    string? Role
);