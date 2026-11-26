using AutoMapper;
using ProductosApi.Entidades;
using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Producto, ProductoDto>()
            .ForMember(d => d.Categoria,
                o => o.MapFrom(s => s.Categoria!.Nombre));

        CreateMap<ProductoCrearDto, Producto>();
    }
}
