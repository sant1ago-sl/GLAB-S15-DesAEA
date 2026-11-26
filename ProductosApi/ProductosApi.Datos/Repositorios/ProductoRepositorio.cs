using Microsoft.EntityFrameworkCore;
using ProductosApi.Entidades;

namespace ProductosApi.Datos.Repositorios;

public class ProductoRepositorio : IProductoRepositorio
{
    private readonly ProductosContext _context;

    public ProductoRepositorio(ProductosContext context) => _context = context;

    public Task<List<Producto>> ListarAsync() =>
        _context.Productos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();

    public Task<Producto?> ObtenerAsync(int id) =>
        _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

    public Task<bool> ExisteNombreAsync(string nombre) =>
        _context.Productos.AnyAsync(p => p.Nombre == nombre);

    public Task<bool> ExisteCategoriaAsync(int categoriaId) =>
        _context.Categorias.AnyAsync(c => c.Id == categoriaId);

    public async Task AgregarAsync(Producto producto) =>
        await _context.Productos.AddAsync(producto);

    public Task EliminarAsync(Producto producto)
    {
        _context.Productos.Remove(producto);
        return Task.CompletedTask;
    }

    public Task GuardarAsync() => _context.SaveChangesAsync();
}
