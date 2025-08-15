using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record LoginRequest(
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    string Nombre,

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    string Password
);