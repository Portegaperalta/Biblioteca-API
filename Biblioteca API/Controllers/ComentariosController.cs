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
        private readonly IRepositorioComentario _repositorioComentario;
        private readonly IUsuarioServicio _usuarioServicio;

        public ComentariosController(IRepositorioComentario repositorioComentario,IUsuarioServicio usuarioServicio)
        {
            _repositorioComentario = repositorioComentario;
            _usuarioServicio = usuarioServicio;
        }

        //GET Comentarios
        [HttpGet]
        [AllowAnonymous]
        [EndpointSummary("Obtiene todos los comentarios del libro")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ComentarioDTO>>> GetComentarios(int libroId)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(libroId);

            if (!existeLibro)
                return NotFound();

            var comentarios = await _repositorioComentario.GetComentariosAsync(libroId);
            var comentariosDTO = comentarios.Select(comentario =>
            new ComentarioDTO
            {
                UsuarioId = comentario.UsuarioId,
                Usuario = comentario.Autor,
                UsuarioEmail = comentario.Usuario.Email,
                Cuerpo = comentario.Cuerpo,
                FechaPublicacion = comentario.FechaPublicacion
            });

            return Ok(comentariosDTO);
        }

        //GET comentario por id
        [HttpGet("{id:guid}", Name = "ObtenerComentario")]
        [AllowAnonymous]
        [EndpointSummary("Obtiene comentario por ID")]
        [EndpointDescription("Obtiene comentario por ID, si el comentario no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ComentarioDTO>> GetComentario([FromRoute] Guid id)
        {
            var comentario = await _repositorioComentario.GetComentarioAsync(id);

            if (comentario is null)
                return NotFound("El comentario no existe");

            var comentarioDTO = new ComentarioDTO
            {
                Id = comentario.Id,
                UsuarioId = comentario.UsuarioId,
                Usuario = comentario.Autor,
                UsuarioEmail = comentario.Usuario.Email,
                Cuerpo = comentario.Cuerpo,
                FechaPublicacion = comentario.FechaPublicacion
            };

            return comentarioDTO;
        }

        //POST comentario
        [HttpPost]
        [EndpointSummary("Crea un comentario")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> PostComentario([FromBody] Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(comentario.LibroId);

            if (!existeLibro)
                return NotFound();


            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null) 
                return NotFound();

            await _repositorioComentario.CreateComentarioAsync(comentario);
            return Created();
        }

        //PUT comentario
        [HttpPut("{id:guid}")]
        [EndpointSummary("Actualiza un comentario por ID")]
        [EndpointDescription("Actualiza comentario por ID, si el ID del comentario en la ruta no coincide con ID de comentario de peticion, devuelve status 400 (Bad Request)")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PutComentario([FromRoute] Guid id, [FromBody] Comentario comentario)
        {
            bool existeLibro = await _repositorioComentario.ExisteLibroAsync(comentario.LibroId);

            if (id != comentario.Id)
                return BadRequest("Los ids de comentario deben coincidir");

            if (!existeLibro)
                return BadRequest($"El libro con id: {comentario.LibroId} no existe");

            await _repositorioComentario.UpdateComentarioAsync(id, comentario);
            return NoContent();
        }

        //PATCH comentario
        [HttpPatch("{id:guid}")]
        [EndpointSummary("Actualiza parcialmente comentario por ID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PatchComentario([FromRoute] Guid id, [FromBody] JsonPatchDocument<ComentarioPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null) 
                return NotFound();

            var comentario = await _repositorioComentario.GetComentarioAsync(id);

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

            await _repositorioComentario.UpdateComentarioAsync(id,comentario);

            return NoContent();
        }

        //DELETE comentario
        [HttpDelete("{id:guid}")]
        [EndpointSummary("Elimina comentario por ID")]
        [EndpointDescription("Elimina comentario por ID, si el comentario o el usuario no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteComentario([FromRoute] Guid id)
        {
            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null) 
                return NotFound();

            var comentarioDb = await _repositorioComentario.GetComentarioAsync(id);

            if (comentarioDb is null) 
                     return NotFound();

            if (comentarioDb.UsuarioId != usuario.Id) 
                return Forbid();
            
            var registrosBorrados = await _repositorioComentario.DeleteComentarioAsync(id);

            if (registrosBorrados <= 0)
                return NotFound("Comentario no encontrado");

            return NoContent();
        }
    }
}
