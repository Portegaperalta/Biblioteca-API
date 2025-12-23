using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly IRepositorioComentario _repositorioComentario;

        public ComentariosController(IRepositorioComentario repositorioComentario)
        {
            _repositorioComentario = repositorioComentario;
        }

        //GET Comentarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentarios(int libroId)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await _repositorioComentario.GetComentariosAsync(libroId);
            return Ok(comentarios);
        }

        //GET comentario por id
        [HttpGet("{id}", Name = "ObtenerComentario")]
        public async Task<ActionResult<Comentario>> GetComentario([FromRoute] Guid comentarioId)
        {
            var comentario = await _repositorioComentario.GetComentarioAsync(comentarioId);

            if (comentario is null)
            {
                return NotFound("El comentario no existe");
            }

            return comentario;
        }

        //POST comentario
        [HttpPost]
        public async Task<ActionResult> PostComentario([FromBody]Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(comentario.LibroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            await _repositorioComentario.CreateComentarioAsync(comentario);
            return Created();
        }

        //PUT comentario
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> PutComentario([FromRoute]Guid id,[FromBody] Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(comentario.LibroId);
            
            if (id != comentario.Id)
            {
                return BadRequest("Los ids de comentario deben coincidir");
            }

            if (!existeLibro)
            {
                return BadRequest($"El libro con id: {comentario.LibroId} no existe");
            }

            await _repositorioComentario.UpdateComentarioAsync(id, comentario);
            return NoContent();
        }
    }
}
