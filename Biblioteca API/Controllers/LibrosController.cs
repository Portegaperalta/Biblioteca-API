using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<Libro>> Get()
        {
            return await _repositorioLibro.GetLibrosAsync();
        }

        // GET: api/libros/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get([FromRoute]int id)
        {
            var libro = await _repositorioLibro.GetLibroAsync(id);

            if (libro is null)
            {
                return NotFound();
            }

            return libro;
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
            return Ok();
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
            return Ok();
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

            return Ok();
        }
    }
}
