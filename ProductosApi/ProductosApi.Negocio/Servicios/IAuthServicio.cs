using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio.Servicios;

public interface IAuthServicio
{
    Task RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);
}
