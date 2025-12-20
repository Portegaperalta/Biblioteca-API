using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly IRepositorioLibro _repositorioLibro;
        public LibrosController(IRepositorioLibro repositorioLibro)
        {
            _repositorioLibro = repositorioLibro;
        }

        // GET: api/libros
        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {
           var libros = await _repositorioLibro.GetLibrosAsync();
           var librosDTO = libros.Select(libro =>
           new LibroDTO
           {
               Id = libro.Id,
               Titulo = libro.Titulo,  
           });

            return librosDTO;
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
                var libroConAutorDTO = new LibroConAutorDTO 
                { Id = libro.Id,
                  Titulo = libro.Titulo,
                  NombreAutor = $"{libro.Autor.Nombres} {libro.Autor.Apellidos}" 
                };

                return libroConAutorDTO;
            }

            var libroDTO = new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
            };

            return libroDTO;
        }

        // POST: api/libros
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Libro libro)
        {
            bool existeAutor = await _repositorioLibro.ExisteAutor(libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"El autor de id: {libro.AutorId} no existe");
            }

            await _repositorioLibro.CreateLibroAsync(libro);
            return CreatedAtRoute("ObtenerLibro", new {id = libro.Id},libro);
        }

        // PUT: api/libros/id
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute]int id,[FromBody]Libro libro)
        {
            bool existeAutor = await _repositorioLibro.ExisteAutor(libro.AutorId);

            if (id != libro.Id)
            {
                return BadRequest($"Los ids deben de coincidir");
            }

            if (!existeAutor)
            {
                return BadRequest($"El autor de id: {libro.AutorId} no existe");
            }

            await _repositorioLibro.UpdateLibroAsync(libro);
            return NoContent();
        }

        // DELETE: api/libros/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            int registrosBorrados = await _repositorioLibro.DeleteLibroAsync(id);

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
