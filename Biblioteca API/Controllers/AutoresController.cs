using Biblioteca_API.DTOs;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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
        [OutputCache]
        [EndpointSummary("Obtiene listado de autores")]
        public async Task<IEnumerable<AutorDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await _autorServicio.GetAutoresDtoAsync(paginacionDTO);
        }

        // GET: api/autores/id
        [HttpGet("{id:int}",Name ="ObtenerAutor")]
        [AllowAnonymous]
        [OutputCache]
        [EndpointSummary("Obtiene autor por ID")]
        [EndpointDescription("Obtiene autor por ID incluyendo sus libros, si el autor no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType<AutorDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AutorDTO>> Get([FromRoute]int id, [FromQuery]bool incluyeLibros)
        {
            var autorDto = await _autorServicio.GetAutorDtoAsync(id);
            
            if (autorDto is null)
            {
                return NotFound();
            }

            return autorDto;
        }

        //GET api/autores/filtrar
        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult> Filtrar([FromQuery] AutorFiltroDTO autorFiltroDTO)
        {
            var autoresFiltrados = await _autorServicio.GetAutoresFiltro(autorFiltroDTO);
        
            return Ok(autoresFiltrados);
        }

        // POST: api/autores
        [HttpPost]
        [EndpointSummary("Crea un autor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromForm] AutorCreacionDTO autorCreacionDto)
        {
            await _autorServicio.CreateAutorAsync(autorCreacionDto);
            return Created();
        }

        // PUT: api/autores/id
        [HttpPut("{id:int}")]
        [EndpointSummary("Actualiza autor por ID")]
        [EndpointDescription("Actualiza autor por ID, si el ID del autor en la ruta no coincide con ID de autor de peticion, devuelve status 400 (Bad Request)")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put([FromRoute] int id,[FromForm] AutorPutDTO autorPutDto)
        {
            if (id != autorPutDto.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            var autorDb = await _autorServicio.GetAutorAsNoTrackingAsync(id);

            if (autorDb is null)
            {
                return NotFound($"El autor con ID: {id} no existe");
            }

            await _autorServicio.UpdateAutorAsync(autorPutDto);
            return NoContent();
        }

        // PATCH: api/autores/id
        //[HttpPatch("{id:int}")]
        //[EndpointSummary("Actualiza parcialmente un autor por ID")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<AutorPatchDTO> patchDoc)
        //{
        //    if (patchDoc is null)
        //    {
        //        return BadRequest();
        //    }

        //    var autor = await _autorServicio.GetAutorAsync(id);

        //    if (autor is null)
        //    {
        //        return NotFound();
        //    }

        //    var autorPatchDTO = _autorServicio.HandleAutorPatchDtoMapping(autor);

        //    patchDoc.ApplyTo(autorPatchDTO, ModelState);

        //    var esValido = TryValidateModel(autorPatchDTO);

        //    if (!esValido)
        //    {
        //        return ValidationProblem();
        //    }

        //    autor.Nombres = autorPatchDTO.Nombres;
        //    autor.Apellidos = autorPatchDTO.Apellidos;

        //    var autorPutDto = _autorServicio.HandleAutorPutDtoMapping(id, autorPatchDTO);
        //    await _autorServicio.UpdateAutorAsync(autorPutDto);

        //    return NoContent();
        //}


        // DELETE: api/autores/id
        [HttpDelete("{id:int}")]
        [EndpointSummary("Elimina un autor por ID")]
        [EndpointDescription("Elimina un autor por ID, si el autor no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            bool autorEliminado = await _autorServicio.DeleteAutorAsync(id);
            
            if (autorEliminado is false) return NotFound();

            return NoContent();
        }
    }
}
