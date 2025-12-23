using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
    }
}
