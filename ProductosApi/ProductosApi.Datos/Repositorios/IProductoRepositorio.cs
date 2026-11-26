using ProductosApi.Entidades;

namespace ProductosApi.Datos.Repositorios;

public interface IProductoRepositorio
{
    Task<List<Producto>> ListarAsync();
    Task<Producto?> ObtenerAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre);
    Task<bool> ExisteCategoriaAsync(int categoriaId);
    Task AgregarAsync(Producto producto);
    Task EliminarAsync(Producto producto);
    Task GuardarAsync();
}
