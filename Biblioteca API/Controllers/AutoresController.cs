using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [ApiController]
    [Route("api/autores")]
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
        public async Task<IEnumerable<AutorDTO>> Get()
        {
            return await _autorServicio.GetAutoresDtoAsync();
        }

        // GET: api/autores/id
        [HttpGet("{id:int}",Name ="ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> Get([FromRoute]int id, [FromQuery]bool incluyeLibros)
        {
            var autorDto = await _autorServicio.GetAutorDtoAsync(id);
            
            if (autorDto is null)
            {
                return NotFound();
            }

            return autorDto;
        }

        // GET: api/autores/primerAutor
        //[HttpGet("primerAutor")]
        //public async Task<AutorDTO> GetPrimerAutor()
        //{
        //    var primerAutor = await _repositorioAutor.GetPrimerAutorAsync();
        //    var primerAutorDTO = new AutorDTO
        //    {
        //        Id = primerAutor.Id,
        //        NombreCompleto = $"{primerAutor.Nombres} {primerAutor.Apellidos}",
        //        Libros = primerAutor.Libros.Select(libro =>
        //        new LibroDTO { Id = libro.LibroId, Titulo = libro.Libro.Titulo }
        //        )
        //    };

        //    return primerAutorDTO;
        //}

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDto)
        {
            await _autorServicio.CreateAutorAsync(autorCreacionDto);
            return Created();
        }

        // PUT: api/autores/id
        [HttpPut("{id:int}")]
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
        //[HttpPatch("{id:int}")]
        //public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<AutorPatchDTO> patchDoc)
        //{
        //    if (patchDoc is null)
        //    {
        //        return BadRequest();
        //    }

        //    var autor = await _repositorioAutor.GetAutorAsync(id);

        //    if (autor is null)
        //    {
        //        return NotFound();
        //    }

        //    var autorPatchDTO = new AutorPatchDTO { Nombres = $"{autor.Nombres}",Apellidos = autor.Apellidos};

        //    patchDoc.ApplyTo(autorPatchDTO,ModelState);

        //    var esValido = TryValidateModel(autorPatchDTO);

        //    if (!esValido)
        //    {
        //        return ValidationProblem();
        //    }

        //    autor.Nombres = autorPatchDTO.Nombres;
        //    autor.Apellidos = autorPatchDTO.Apellidos;

        //    await _repositorioAutor.UpdateAutorAsync(autor);

        //    return NoContent();
        //}


        // DELETE: api/autores/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            var registrosBorrados = await _repositorioAutor.DeleteAutorAsync(id);
            
            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
