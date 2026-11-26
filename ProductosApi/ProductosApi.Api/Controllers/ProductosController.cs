using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductosApi.Entidades.Dtos;
using ProductosApi.Negocio;
using ProductosApi.Negocio.Servicios;

namespace ProductosApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoServicio _servicio;

    public ProductosController(IProductoServicio servicio) => _servicio = servicio;

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> Get() =>
        Ok(await _servicio.ListarAsync());

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Get(int id)
    {
        var producto = await _servicio.ObtenerAsync(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Post(ProductoCrearDto dto)
    {
        try
        {
            var creado = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, ProductoCrearDto dto)
    {
        try
        {
            if (!await _servicio.ActualizarAsync(id, dto)) return NotFound();
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _servicio.EliminarAsync(id)) return NotFound();
        return NoContent();
    }
}
