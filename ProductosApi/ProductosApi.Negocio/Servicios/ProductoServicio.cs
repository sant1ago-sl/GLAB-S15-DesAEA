using AutoMapper;
using ProductosApi.Datos.Repositorios;
using ProductosApi.Entidades;
using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio.Servicios;

public class ProductoServicio : IProductoServicio
{
    private readonly IProductoRepositorio _repo;
    private readonly IMapper _mapper;

    public ProductoServicio(IProductoRepositorio repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<ProductoDto>> ListarAsync() =>
        _mapper.Map<List<ProductoDto>>(await _repo.ListarAsync());

    public async Task<ProductoDto?> ObtenerAsync(int id)
    {
        var producto = await _repo.ObtenerAsync(id);
        return producto == null ? null : _mapper.Map<ProductoDto>(producto);
    }

    public async Task<ProductoDto> CrearAsync(ProductoCrearDto dto)
    {
        if (await _repo.ExisteNombreAsync(dto.Nombre))
            throw new ReglaNegocioException("Ya existe un producto con ese nombre.");
        if (!await _repo.ExisteCategoriaAsync(dto.CategoriaId))
            throw new ReglaNegocioException("La categoría indicada no existe.");

        var producto = _mapper.Map<Producto>(dto);
        await _repo.AgregarAsync(producto);
        await _repo.GuardarAsync();

        return _mapper.Map<ProductoDto>(await _repo.ObtenerAsync(producto.Id));
    }

    public async Task<bool> ActualizarAsync(int id, ProductoCrearDto dto)
    {
        var producto = await _repo.ObtenerAsync(id);
        if (producto == null) return false;

        if (!await _repo.ExisteCategoriaAsync(dto.CategoriaId))
            throw new ReglaNegocioException("La categoría indicada no existe.");

        _mapper.Map(dto, producto);
        await _repo.GuardarAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _repo.ObtenerAsync(id);
        if (producto == null) return false;

        await _repo.EliminarAsync(producto);
        await _repo.GuardarAsync();
        return true;
    }
}
