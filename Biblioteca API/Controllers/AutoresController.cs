using Biblioteca_API.DTOs;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(Policy = "esAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly IAutorServicio _autorServicio;

        public AutoresController(IAutorServicio autorServicio)
        {
            _autorServicio = autorServicio;
        }

        // GET: api/autores
        [HttpGet]
        [HttpGet("/listado-de-autores")]
        [AllowAnonymous]
        [EndpointSummary("Obtiene listado de autores")]
        public async Task<IEnumerable<AutorDTO>> Get()
        {
            return await _autorServicio.GetAutoresDtoAsync();
        }

        // GET: api/autores/id
        [HttpGet("{id:int}",Name ="ObtenerAutor")]
        [AllowAnonymous]
        [EndpointSummary("Obtiene autor por ID")]
        [EndpointDescription("Obtiene autor por ID incluyendo sus libros, si el autor no existe, devuelve status 404 (Not Found)")]
        public async Task<ActionResult<AutorDTO>> Get([FromRoute]int id, [FromQuery]bool incluyeLibros)
        {
            var autorDto = await _autorServicio.GetAutorDtoAsync(id);
            
            if (autorDto is null)
            {
                return NotFound();
            }

            return autorDto;
        }

        // POST: api/autores
        [HttpPost]
        [EndpointSummary("Crea un autor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDto)
        {
            await _autorServicio.CreateAutorAsync(autorCreacionDto);
            return Created();
        }

        // PUT: api/autores/id
        [HttpPut("{id:int}")]
        [EndpointSummary("Actualiza autor por ID")]
        [EndpointDescription("Actualiza autor por ID, si el ID del autor en la ruta no coincide con ID de autor de peticion, devuelve status 400 (Bad Request)")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] AutorPutDTO autorPutDto)
        {
            if (id != autorPutDto.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            await _autorServicio.UpdateAutorAsync(autorPutDto);
            return NoContent();
        }

        // PATCH: api/autores/id
        [HttpPatch("{id:int}")]
        [EndpointSummary("Actualiza parcialmente un autor por ID")]
        public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<AutorPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var autor = await _autorServicio.GetAutorAsync(id);

            if (autor is null)
            {
                return NotFound();
            }

            var autorPatchDTO = _autorServicio.HandleAutorPatchDtoMapping(autor);

            patchDoc.ApplyTo(autorPatchDTO, ModelState);

            var esValido = TryValidateModel(autorPatchDTO);

            if (!esValido)
            {
                return ValidationProblem();
            }

            autor.Nombres = autorPatchDTO.Nombres;
            autor.Apellidos = autorPatchDTO.Apellidos;

            var autorPutDto = _autorServicio.HandleAutorPutDtoMapping(id, autorPatchDTO);
            await _autorServicio.UpdateAutorAsync(autorPutDto);

            return NoContent();
        }


        // DELETE: api/autores/id
        [HttpDelete("{id:int}")]
        [EndpointSummary("Elimina un autor por ID")]
        [EndpointDescription("Elimina un autor por ID, si el autor no existe, devuelve status 404 (Not Found)")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            var registrosBorrados = await _autorServicio.DeleteAutorAsync(id);
            
            if (registrosBorrados == 0) return NotFound();

            return NoContent();
        }
    }
}
