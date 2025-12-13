using Biblioteca_API.Datos;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
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

        // POST: api/libros
        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
