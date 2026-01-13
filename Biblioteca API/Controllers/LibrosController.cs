using Biblioteca_API.DTOs;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    [Authorize(Policy = "esAdmin")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroServicio _libroServicio;
        private readonly ITimeLimitedDataProtector _protectorLimitadoPorTiempo;
        private readonly IOutputCacheStore _outputCacheStore;
        private const string cache = "libros-obtener";

        public LibrosController(ILibroServicio libroServicio, 
            IDataProtectionProvider protectionProvider, 
            IOutputCacheStore outputCacheStore)
        {
            _libroServicio = libroServicio;
            _protectorLimitadoPorTiempo = protectionProvider
                                         .CreateProtector("LibrosController")
                                         .ToTimeLimitedDataProtector();

            _outputCacheStore = outputCacheStore;
        }

        // GET: api/libros
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(Tags = [cache])]
        [EndpointSummary("Obtiene listado de libros")]
        public async Task<IEnumerable<LibroDTO>> GetAll([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await _libroServicio.GetLibrosDtoAsync(paginacionDTO);
        }

        // GET: api/libros/id
        [HttpGet("{id:int}",Name ="ObtenerLibro")]
        [AllowAnonymous]
        [OutputCache(Tags = [cache])]
        [EndpointSummary("Obtiene libro por ID")]
        [EndpointDescription("Obtiene libro por ID, si el libro no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        // POST: api/libros
        [HttpPost]
        [EndpointSummary("Crea un libro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] LibroCreacionDTO libroCreacionDTO)
        {
            await _libroServicio.CreateLibroAsync(libroCreacionDTO);
            await _outputCacheStore.EvictByTagAsync(cache, default);

            return Created();
        }

        // PUT: api/libros/id
        [HttpPut("{id:int}")]
        [EndpointSummary("Actualiza libro por ID")]
        [EndpointDescription("Actualiza libro por ID, si el ID del libro en la ruta no coincide con ID de libro de peticion, devuelve status 400 (Bad Request)")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put([FromRoute]int id,[FromForm] LibroPutDTO libroPutDto)
        {
            if (id != libroPutDto.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            await _libroServicio.UpdateLibroAsync(id,libroPutDto);
            await _outputCacheStore.EvictByTagAsync(cache, default);

            return NoContent();
        }

        // DELETE: api/libros/id
        [HttpDelete("{id:int}")]
        [EndpointSummary("Elimina libro por ID")]
        [EndpointDescription("Elimina comentario por ID, si el comentario no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            int registrosBorrados = await _libroServicio.DeleteLibroAsync(id);

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            await _outputCacheStore.EvictByTagAsync(cache, default);

            return NoContent();
        }
    }
}
