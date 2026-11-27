# Semana 15: Microservicios con API Gateway y Ocelot

## 1. Explicación del tema

El tema es una **tienda de tecnología**.

- **Microservicio 1, ProductosApi (API de la S14):** administra el catálogo de productos por categorías (laptops, accesorios y monitores) en SQL Server. Permite registrarse e iniciar sesión con JWT. La consulta de productos es pública; crear y actualizar exige token, y eliminar exige el rol `Admin`. Está organizada en capas (Api, Negocio, Datos y Entidades).
- **Microservicio 2, ClientesApi:** servicio pequeño con los clientes de la tienda, con datos en memoria. Tiene dos endpoints GET (lista y por id) y ambos exigen token.
- **GatewayApi:** API Gateway con Ocelot. Es el único punto de entrada para el cliente y valida el JWT antes de reenviar las rutas protegidas.

## 2. Tabla de rutas

| Upstream (cliente → gateway) | Método | Downstream local | Downstream en la nube | Token |
|---|---|---|---|---|
| `/gateway/auth/{everything}` | POST | `localhost:5055/api/auth/{everything}` | `https://productos-santiago.runasp.net/api/auth/{everything}` | No |
| `/gateway/productos{everything}` | GET | `localhost:5055/api/productos{everything}` | `https://productos-santiago.runasp.net/api/productos{everything}` | No |
| `/gateway/productos{everything}` | POST, PUT, DELETE | `localhost:5055/api/productos{everything}` | `https://productos-santiago.runasp.net/api/productos{everything}` | Sí |
| `/gateway/clientes{everything}` | GET | `localhost:5056/api/clientes{everything}` | `https://clientes-santiago.runasp.net/api/clientes{everything}` | Sí |

## 3. Enlace del repositorio

https://github.com/sant1ago-sl/GLAB-S15-DesAEA

- Scripts SQL: `ProductosApi/ProductosDB.sql`, `ProductosApi/Usuarios.sql` y `ProductosApi/ProductosDB.nube.sql`.
- Configuración de rutas: `GatewayApi/ocelot.json` (local) y `GatewayApi/ocelot.Production.json` (nube).
- La clave JWT y las cadenas de conexión no están en los archivos del repositorio: en local se usan *user-secrets* y en la nube variables de entorno.

## 4. ocelot.json (versión publicada)

Archivo `GatewayApi/ocelot.Production.json`:

```json
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/gateway/auth/{everything}",
      "UpstreamHttpMethod": [ "Post" ],
      "DownstreamPathTemplate": "/api/auth/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [ { "Host": "productos-santiago.runasp.net", "Port": 443 } ]
    },
    {
      "UpstreamPathTemplate": "/gateway/productos{everything}",
      "UpstreamHttpMethod": [ "Get" ],
      "DownstreamPathTemplate": "/api/productos{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [ { "Host": "productos-santiago.runasp.net", "Port": 443 } ]
    },
    {
      "UpstreamPathTemplate": "/gateway/productos{everything}",
      "UpstreamHttpMethod": [ "Post", "Put", "Delete" ],
      "DownstreamPathTemplate": "/api/productos{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [ { "Host": "productos-santiago.runasp.net", "Port": 443 } ],
      "AuthenticationOptions": { "AuthenticationProviderKeys": [ "Bearer" ] }
    },
    {
      "UpstreamPathTemplate": "/gateway/clientes{everything}",
      "UpstreamHttpMethod": [ "Get" ],
      "DownstreamPathTemplate": "/api/clientes{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [ { "Host": "clientes-santiago.runasp.net", "Port": 443 } ],
      "AuthenticationOptions": { "AuthenticationProviderKeys": [ "Bearer" ] }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://gateway-santiago.runasp.net"
  }
}
```

**[PEGAR CAPTURA DEL ARCHIVO]**

## 5. Los tres servicios en ejecución en el equipo

| Servicio | Puerto |
|---|---|
| GatewayApi | 5000 |
| ProductosApi | 5055 |
| ClientesApi | 5056 |

**[PEGAR CAPTURA DE LAS TRES TERMINALES]**

## 6. Publicación en la nube

| Pieza | Enlace | Proveedor |
|---|---|---|
| Gateway | https://gateway-santiago.runasp.net/gateway/productos | MonsterASP.NET |
| ProductosApi | https://productos-santiago.runasp.net/api/productos | MonsterASP.NET |
| ClientesApi (exige token) | https://clientes-santiago.runasp.net/api/clientes | MonsterASP.NET |
| Base de datos | `db82796.databaseasp.net` (acceso interno) | MonsterASP.NET, SQL Server 2025 |

**Justificación de la elección**

- El plan gratuito de MonsterASP.NET no pidió tarjeta. Azure, la primera alternativa evaluada, sí la exigía para crear la cuenta.
- Incluye hasta 5 sitios y 5 bases de datos gratuitas, suficiente para los tres servicios y la base.
- Soporta ASP.NET Core con .NET 10 sin necesidad de Docker.
- Ofrece SQL Server, así que no hubo que cambiar el proveedor de EF Core ni la estructura del script.
- Permite variables de entorno para los secretos y certificados HTTPS gratuitos de Let's Encrypt.
- Limitaciones: 256 MB de memoria por sitio, 1 GB por base de datos, centro de datos solo en Europa y sin garantías de producción.

Otra alternativa revisada fue Render con Neon (PostgreSQL), descartada porque exigía Docker y cambiar el proveedor de base de datos.

## 7. Pruebas por el gateway

| Código | Caso | En el equipo | En la nube |
|---|---|---|---|
| 200 | GET `/gateway/productos` | [captura] | [captura] |
| 201 | POST `/gateway/productos` con token | [captura] | [captura] |
| 204 | PUT o DELETE con token de admin | [captura] | [captura] |
| 401 | POST `/gateway/productos` o GET `/gateway/clientes` sin token | [captura] | [captura] |
| 403 | DELETE con rol `Usuario` | [captura] | [captura] |
| 404 | GET `/gateway/productos/999` | [captura] | [captura] |
| 502 | GET `/gateway/clientes` con el servicio inalcanzable | [captura] | [captura] |
| 401 | GET directo a ClientesApi sin token | [captura] | [captura] |

**Nota sobre el 502 en la nube:** al detener ClientesApi desde el panel, el gateway respondió **503**, porque el hosting sigue contestando con una página de "sitio fuera de línea" y el gateway la reenvía. El 502 aparece cuando el gateway no logra conectarse con el servicio.

## 8. Video de sustentación

**[PEGAR ENLACE DEL VIDEO]** (mínimo 10 minutos, cámara encendida, acceso para cualquiera con el enlace)

## 9. Observaciones

1. El plan gratuito ejecuta las aplicaciones en 32 bits (x86) por defecto, así que se publicaron con `-r win-x86`.
2. El dominio gratuito no traía HTTPS activo: hubo que habilitar el certificado en cada uno de los tres sitios.
3. Sin las variables de entorno, las aplicaciones arrancaban pero respondían error 500.
4. Al detener un servicio en la nube se obtuvo 503 y no 502, a diferencia de la prueba local.
5. Con ClientesApi detenido, las rutas de productos siguieron respondiendo 200.

## 10. Conclusiones

1. El gateway ofrece al cliente una sola dirección y oculta cuántos servicios existen y dónde están.
2. `ocelot.json` permite cambiar los destinos (de `localhost` a los hosts públicos) sin modificar el código del cliente ni de los servicios.
3. Validar el JWT en el gateway y en cada servicio protege también las llamadas directas, pero obliga a mantener la misma clave, emisor y audiencia en tres lugares.
4. Los microservicios fallan de forma independiente, a cambio de una mayor complejidad de despliegue y configuración.
5. Mantener SQL Server en la nube evitó cambiar el proveedor de EF Core; los secretos quedaron fuera del código mediante *user-secrets* y variables de entorno.
