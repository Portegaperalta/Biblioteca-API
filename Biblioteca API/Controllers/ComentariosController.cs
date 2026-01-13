using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    [Authorize]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentarioServicio _comentarioServicio;
        private readonly IUsuarioServicio _usuarioServicio;

        public ComentariosController(IComentarioServicio comentarioServicio,IUsuarioServicio usuarioServicio)
        {
            _comentarioServicio = comentarioServicio;
            _usuarioServicio = usuarioServicio;
        }

        //GET Comentarios
        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Obtiene todos los comentarios del libro")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ComentarioDTO>>> GetAll(int libroId)
        {
            var comentariosDTO = await _comentarioServicio.GetAllAsync(libroId);
            return Ok(comentariosDTO);
        }

        //GET comentario por id
        [HttpGet("{id:guid}", Name = "ObtenerComentario")]
        [AllowAnonymous]
        [EndpointSummary("Obtiene comentario por ID")]
        [EndpointDescription("Obtiene comentario por ID, si el comentario no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ComentarioDTO>> Get([FromRoute] Guid id)
        {
            var comentarioDTO = await _comentarioServicio.GetByIdAsync(id);

            if (comentarioDTO is null)
            {
                return NotFound("El comentario no existe");
            }
                
            return comentarioDTO;
        }

        //POST comentario
        [HttpPost]
        [EndpointSummary("Crea un comentario")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] Comentario comentario)
        {
            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null)
            {
               return Unauthorized();
            }

            await _comentarioServicio.CreateAsync(comentario.LibroId, comentario);
            
            return Created();
        }

        //PUT comentario
        [HttpPut("{id:guid}")]
        [EndpointSummary("Actualiza un comentario por ID")]
        [EndpointDescription("Actualiza comentario por ID, si el ID del comentario en la ruta no coincide con ID de comentario de peticion, devuelve status 400 (Bad Request)")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] Comentario comentario)
        {
            if (id != comentario.Id)
                return BadRequest("Los ids de comentario deben coincidir");

            await _comentarioServicio.UpdateAsync(comentario);
            return NoContent();
        }

        //PATCH comentario
        [HttpPatch("{id:guid}")]
        [EndpointSummary("Actualiza parcialmente comentario por ID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument<ComentarioPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null) 
                return NotFound();

            var comentario = await _comentarioServicio.GetEntityByIdAsync(id);

            if (comentario is null)
                return NotFound("Comentario no encontrado");

            if (comentario.UsuarioId != usuario.Id)
                return Forbid();

            var comentarioPatchDTO = new ComentarioPatchDTO { Cuerpo = comentario.Cuerpo };

            patchDoc.ApplyTo(comentarioPatchDTO,ModelState);

            bool esValido = TryValidateModel(comentarioPatchDTO);

            if (!esValido)
                return ValidationProblem();

            comentario.Cuerpo = comentarioPatchDTO.Cuerpo;

            await _comentarioServicio.UpdateAsync(comentario);

            return NoContent();
        }

        //DELETE comentario
        [HttpDelete("{id:guid}")]
        [EndpointSummary("Elimina comentario por ID")]
        [EndpointDescription("Elimina comentario por ID, si el comentario o el usuario no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null)
            {
                return Unauthorized();
            }

            var comentarioDTO = await _comentarioServicio.GetByIdAsync(id);

            if (comentarioDTO is null)
            {
                return NotFound();
            }

            if (comentarioDTO.UsuarioId != usuario.Id)
            {
                return Forbid();
            }

            bool estaBorrado = await _comentarioServicio.DeleteAsync(id);

            if (estaBorrado == false)
            {
                return NotFound("Comentario no encontrado");
            }

            return NoContent();
        }
    }
}
