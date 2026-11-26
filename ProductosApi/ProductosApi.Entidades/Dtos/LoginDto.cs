using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Entidades.Dtos;

public class LoginDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = null!;

    [Required]
    public string Clave { get; set; } = null!;
}
