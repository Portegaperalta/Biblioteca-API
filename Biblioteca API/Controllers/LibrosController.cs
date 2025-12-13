using Biblioteca_API.Datos;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/libros
        [HttpGet]
        public async Task<IEnumerable<Libro>> Get()
        {
            return await context.Libros.ToListAsync();
        }

        // GET: api/libros/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            var libro = await context.Libros
                .Include(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro is null)
            {
                return NotFound();
            }

            return libro;
        }

        // POST: api/libros
        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"El autor de id: {libro.AutorId} no existe");
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/libros/id
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest($"Los ids deben de coincidir");
            }

            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"El autor de id: {libro.AutorId} no existe");
            }

            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/libros/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
