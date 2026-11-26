namespace ProductosApi.Entidades.Dtos;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public string? Categoria { get; set; }
}
