using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        [HttpGet]
        public IEnumerable<Autor> Get()
        {
            return new List<Autor>
            {
                new Autor{Id = 1,Nombre = "Pablo Neruda"},
                new Autor{Id = 2, Nombre = "Ernest Hemingway"}
            };
        }
    }
}
