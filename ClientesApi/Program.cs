using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true, ValidIssuer = jwt["Emisor"],
            ValidateAudience = true, ValidAudience = jwt["Audiencia"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Clave"]!))
        });
builder.Services.AddAuthorization();

var clientes = new List<Cliente>
{
    new(1, "Ana Torres", "ana@correo.com"),
    new(2, "Luis Rojas", "luis@correo.com"),
    new(3, "Marta Díaz", "marta@correo.com")
};

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/clientes", () => clientes).RequireAuthorization();

app.MapGet("/api/clientes/{id}", (int id) =>
    clientes.FirstOrDefault(c => c.Id == id) is { } cliente
        ? Results.Ok(cliente)
        : Results.NotFound()).RequireAuthorization();

app.Run();

record Cliente(int Id, string Nombre, string Correo);
