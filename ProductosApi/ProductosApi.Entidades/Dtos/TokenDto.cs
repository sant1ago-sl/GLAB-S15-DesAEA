namespace ProductosApi.Entidades.Dtos;

public class TokenDto
{
    public string Token { get; set; } = null!;
    public DateTime Expira { get; set; }
}
