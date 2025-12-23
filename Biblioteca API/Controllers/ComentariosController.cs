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
    }
}
