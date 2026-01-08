using Biblioteca_API.DTOs;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    [Authorize(Policy = "esAdmin")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroServicio _libroServicio;
        private readonly ITimeLimitedDataProtector _protectorLimitadoPorTiempo;

        public LibrosController(ILibroServicio libroServicio,IDataProtectionProvider protectionProvider)
        {
            _libroServicio = libroServicio;
            _protectorLimitadoPorTiempo = protectionProvider
                                         .CreateProtector("LibrosController")
                                         .ToTimeLimitedDataProtector();
        }

        // GET: api/libros
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var librosDto = await _libroServicio.GetLibrosDtoAsync();
            return librosDto;
        }

        // GET: api/libros/id
        [HttpGet("{id:int}",Name ="ObtenerLibro")]
        [AllowAnonymous]
        public async Task<ActionResult<LibroDTO>> Get([FromRoute]int id, [FromQuery]bool incluyeAutor)
        {
            var libro = await _libroServicio.GetLibroAsync(id);

            if (libro is null)
            {
                return NotFound();
            }

            if (incluyeAutor == true)
            {
                var libroConAutores = await _libroServicio.GetLibroConAutoresDto(libro.Id);

                if (libroConAutores is null)
                {
                    return NotFound();
                }

                return libroConAutores;
            }

            var libroDto = await _libroServicio.GetLibroDtoAsync(id);

            return libroDto;
        }

        [HttpGet("creacion/obtener-token")]
        public ActionResult ObtenerTokenListado()
        {
            var textoPlano = Guid.NewGuid().ToString();
            var token = _protectorLimitadoPorTiempo
                        .Protect(textoPlano,lifetime: TimeSpan.FromSeconds(30));
            var url = Url.RouteUrl("CrearLibroUsandoToken",new {token},"https");

            return Ok(new { url });
        }

        // POST: api/libros
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LibroCreacionDTO libroCreacionDTO)
        {
            await _libroServicio.CreateLibroAsync(libroCreacionDTO);
            return Created();
        }

        // PUT: api/libros/id
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute]int id,[FromBody] LibroPutDTO libroPutDto)
        {
            await _libroServicio.UpdateLibroAsync(id,libroPutDto);
            return NoContent();
        }

        // DELETE: api/libros/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            int registrosBorrados = await _libroServicio.DeleteLibroAsync(id);

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
