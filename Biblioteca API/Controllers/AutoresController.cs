using Biblioteca_API.Datos;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/autores
        [HttpGet]
        [HttpGet("/listado-de-autores")]
        public async Task<IEnumerable<Autor>> Get()
        {
            return await context.Autores.ToListAsync();
        }

        // GET: api/autores/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get([FromRoute]int id, [FromQuery]bool incluyeLibros)
        {
            var autor = await context.Autores
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);

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
            return await context.Autores.FirstAsync();
        }

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
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

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/autores/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            var registrosBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
            
            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
