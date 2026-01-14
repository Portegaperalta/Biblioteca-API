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

            return datosHateoas;
        }
    }
}
