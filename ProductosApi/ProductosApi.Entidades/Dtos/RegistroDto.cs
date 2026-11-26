using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Entidades.Dtos;

public class RegistroDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = null!;

    [Required, StringLength(50, MinimumLength = 6)]
    public string Clave { get; set; } = null!;
}
