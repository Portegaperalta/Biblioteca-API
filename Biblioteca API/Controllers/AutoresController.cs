using System.Reflection.Metadata.Ecma335;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly IRepositorioAutor _repositorioAutor;

        public AutoresController(IRepositorioAutor repositorioAutor)
        {
            _repositorioAutor = repositorioAutor;
        }

        // GET: api/autores
        [HttpGet]
        [HttpGet("/listado-de-autores")]
        public async Task<IEnumerable<AutorDTO>> Get()
        {
            var autores = await _repositorioAutor.GetAutoresAsync();
            var autoresDTO = autores.Select(autor =>
            new AutorDTO 
            { 
                Id = autor.Id, 
                NombreCompleto = $"{autor.Nombres} {autor.Apellidos}",
                Libros = autor.Libros.Select(libro =>
                new LibroDTO { Id = libro.Id, Titulo = libro.Titulo}
                )
            });

            return autoresDTO;
        }

        // GET: api/autores/id
        [HttpGet("{id:int}",Name ="ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> Get([FromRoute]int id, [FromQuery]bool incluyeLibros)
        {
            var autor = await _repositorioAutor.GetAutorAsync(id);

            if (autor is null)
            {
                return NotFound();
            }

            var autorDTO = new AutorDTO
            {
                Id = autor.Id,
                NombreCompleto = $"{autor.Nombres} {autor.Apellidos}",
                Libros = autor.Libros.Select(libro =>
                new LibroDTO { Id = libro.Id, Titulo = libro.Titulo }
                )
            };

            return autorDTO;
        }

        // GET: api/autores/primerAutor
        [HttpGet("primerAutor")]
        public async Task<AutorDTO> GetPrimerAutor()
        {
            var primerAutor = await _repositorioAutor.GetPrimerAutorAsync();
            var primerAutorDTO = new AutorDTO
            {
                Id = primerAutor.Id,
                NombreCompleto = $"{primerAutor.Nombres} {primerAutor.Apellidos}",
                Libros = primerAutor.Libros.Select(libro =>
                new LibroDTO { Id = libro.Id, Titulo = libro.Titulo }
                )
            };

            return primerAutorDTO;
        }

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            await _repositorioAutor.CreateAutorAsync(autor);
            return CreatedAtRoute("ObtenerAutor",new {id = autor.Id}, autor);
        }

        // PUT: api/autores/id
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            await _repositorioAutor.UpdateAutorAsync(autor);
            return NoContent();
        }

        // DELETE: api/autores/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            var registrosBorrados = await _repositorioAutor.DeleteAutorAsync(id);
            
            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return Ok("Autor eliminado exitosamente");
        }
    }
}
