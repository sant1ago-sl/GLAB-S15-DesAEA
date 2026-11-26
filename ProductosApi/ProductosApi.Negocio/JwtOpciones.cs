namespace ProductosApi.Negocio;

public class JwtOpciones
{
    public string Clave { get; set; } = null!;
    public string Emisor { get; set; } = null!;
    public string Audiencia { get; set; } = null!;
    public int Minutos { get; set; }
}
