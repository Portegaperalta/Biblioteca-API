using Biblioteca_API.Datos;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/autores
        [HttpGet]
        public IEnumerable<Autor> Get()
        {
            return new List<Autor>
            {
                new Autor{Id = 1,Nombre = "Pablo Neruda"},
                new Autor{Id = 2, Nombre = "Ernest Hemingway"}
            };
        }

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
