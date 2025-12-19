using Biblioteca_API.Datos.Repositorios;
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
        public async Task<IEnumerable<Autor>> Get()
        {
            return await _repositorioAutor.GetAutoresAsync();
        }

        // GET: api/autores/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get([FromRoute]int id, [FromQuery]bool incluyeLibros)
        {
            var autor = await _repositorioAutor.GetAutorAsync(id);

            if (autor is null)
            {
                return NotFound();
            }

            return autor;
        }

        // GET: api/autores/primerAutor
        [HttpGet("primerAutor")]
        public async Task<Autor> GetPrimerAutor()
        {
            return await _repositorioAutor.GetPrimerAutorAsync();
        }

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            await _repositorioAutor.CreateAutorAsync(autor);
            return Ok();
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
            return Ok();
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

            return Ok();
        }
    }
}
