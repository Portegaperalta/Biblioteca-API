using Biblioteca_API.DTOs;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroServicio _libroServicio;
        public LibrosController(ILibroServicio libroServicio)
        {
            _libroServicio = libroServicio;
        }

        // GET: api/libros
        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var librosDto = await _libroServicio.GetLibrosAsync();
            return librosDto;
        }

        // GET: api/libros/id
        [HttpGet("{id:int}",Name ="ObtenerLibro")]
        public async Task<ActionResult<LibroDTO>> Get([FromRoute]int id, [FromQuery]bool incluyeAutor)
        {
            var libro = await _repositorioLibro.GetLibroAsync(id);

            if (libro is null)
            {
                return NotFound();
            }

            if (incluyeAutor == true)
            {
                var libroConAutorDTO = new LibroConAutoresDTO 
                { Id = libro.Id,
                  Titulo = libro.Titulo,
                  NombreAutores = libro.Autores
                };

                return libroConAutorDTO;
            }

            var libroDto = await _libroServicio.GetLibroAsync(id);

            return libroDto;
        }

        // POST: api/libros
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LibroCreacionDTO libroCreacionDTO)
        {
            await _libroServicio.CreateLibroAsync(libroCreacionDTO);
            return Created();
        }

        // PUT: api/libros/id
        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put([FromRoute]int id,[FromBody]Libro libro)
        //{
        //    bool existeAutor = await _repositorioLibro.ExisteAutor(libro.AutorId);

        //    if (id != libro.Id)
        //    {
        //        return BadRequest($"Los ids deben de coincidir");
        //    }

        //    if (!existeAutor)
        //    {
        //        return BadRequest($"El autor de id: {libro.AutorId} no existe");
        //    }

        //    await _repositorioLibro.UpdateLibroAsync(libro);
        //    return NoContent();
        //}

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
