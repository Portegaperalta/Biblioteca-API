using Biblioteca_API.DTOs.HATEOAS;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    public class RootController : ControllerBase
    {
        [HttpGet("ObtenerRootV1")]
        public IEnumerable<DatosHATEOASDTO> Get()
        {
            var datosHateoas = new List<DatosHATEOASDTO>();

            datosHateoas.Add(new DatosHATEOASDTO(
                                 Enlace: Url.Link("ObtenerRootV1", new {})!,
                                 Descripcion: "self",
                                 Metodo:"GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerAutoresV1",new {})!,
                Descripcion:"autores-obtener",
                Metodo:"GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("CrearAutorV1", new { })!,
                Descripcion: "autor-crear",
                Metodo: "POST"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerLibrosV1", new { })!,
                Descripcion: "libros-obtener",
                Metodo: "GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("CrearLibroV1", new { })!,
                Descripcion: "libro-crear",
                Metodo: "POST"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerComentariosV1", new { })!,
                Descripcion: "comentarios-obtener",
                Metodo: "GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("CrearComentarioV1", new { })!,
                Descripcion: "comentario-crear",
                Metodo: "POST"));
            return datosHateoas;
        }
    }
}
