using Microsoft.EntityFrameworkCore;
using ProductosApi.Entidades;

namespace ProductosApi.Datos.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly ProductosContext _context;

    public UsuarioRepositorio(ProductosContext context) => _context = context;

    public Task<Usuario?> ObtenerPorCorreoAsync(string correo) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

    public async Task AgregarAsync(Usuario usuario) =>
        await _context.Usuarios.AddAsync(usuario);

    public Task GuardarAsync() => _context.SaveChangesAsync();
}
