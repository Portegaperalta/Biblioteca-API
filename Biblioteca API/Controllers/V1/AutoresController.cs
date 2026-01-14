using Biblioteca_API.DTOs;
using Biblioteca_API.DTOs.Autor;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Biblioteca_API.Controllers.V1
{
    [ApiController]
    [Route("api/v1/autores")]
    [Authorize(Policy = "esAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly IAutorServicio _autorServicio;
        private readonly IOutputCacheStore _outputCacheStore;
        private const string cache = "autores-obtener" ;

        public AutoresController(IAutorServicio autorServicio,IOutputCacheStore outputCacheStore)
        {
            _autorServicio = autorServicio;
            _outputCacheStore = outputCacheStore;
        }

        // GET: api/v1/autores
        [HttpGet(Name = "ObtenerAutoresV1")]
        [HttpGet("listado-de-autores")]
        [AllowAnonymous]
        [OutputCache(Tags = [cache])]
        [EndpointSummary("Obtiene listado de autores")]
        public async Task<IEnumerable<AutorDTO>> GetAll([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await _autorServicio.GetAutoresDtoAsync(paginacionDTO);
        }

        // GET: api/v1/autores/id
        [HttpGet("{id:int}",Name ="ObtenerAutorV1")]
        [AllowAnonymous]
        [OutputCache(Tags = [cache])]
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

        //GET api/v1/autores/filtrar
        [HttpGet("filtrar",Name = "FiltrarAutoresV1")]
        [AllowAnonymous]
        public async Task<ActionResult> Filtrar([FromQuery] AutorFiltroDTO autorFiltroDTO)
        {
            var autoresFiltrados = await _autorServicio.GetAutoresFiltro(autorFiltroDTO);
        
            return Ok(autoresFiltrados);
        }

        // POST: api/v1/autores
        [HttpPost(Name = "CrearAutorV1")]
        [EndpointSummary("Crea un autor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromForm] AutorCreacionDTO autorCreacionDto)
        {
            await _autorServicio.CreateAutorAsync(autorCreacionDto);
            await _outputCacheStore.EvictByTagAsync(cache, default);
            
            return Created();
        }

        // PUT: api/v1/autores/id
        [HttpPut("{id:int}",Name = "ActualizarAutorV1")]
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
            await _outputCacheStore.EvictByTagAsync(cache, default);

            return NoContent();
        }

        // PATCH: api/v1/autores/id
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


        // DELETE: api/v1/autores/id
        [HttpDelete("{id:int}",Name = "BorrarAutorV1")]
        [EndpointSummary("Elimina un autor por ID")]
        [EndpointDescription("Elimina un autor por ID, si el autor no existe, devuelve status 404 (Not Found)")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            bool autorEliminado = await _autorServicio.DeleteAutorAsync(id);
            
            if (autorEliminado is false) return NotFound();

            await _outputCacheStore.EvictByTagAsync(cache, default);

            return NoContent();
        }
    }
}
