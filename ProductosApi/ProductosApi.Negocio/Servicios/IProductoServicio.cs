using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio.Servicios;

public interface IProductoServicio
{
    Task<List<ProductoDto>> ListarAsync();
    Task<ProductoDto?> ObtenerAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoCrearDto dto);
    Task<bool> ActualizarAsync(int id, ProductoCrearDto dto);
    Task<bool> EliminarAsync(int id);
}
