using Biblioteca_API.DTOs;
using Biblioteca_API.DTOs.Autor;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.JsonPatch;

namespace Biblioteca_API.Servicios
{
    public interface IAutorServicio
    {
        Task<IEnumerable<AutorDTO>> GetAutoresDtoAsync(PaginacionDTO paginacionDTO);
        Task<AutorDTO?> GetAutorDtoAsync(int autorId);
        Task<Autor?> GetAutorAsync(int autorId);
        Task<AutorSinLibrosDTO?> GetAutorSinLibrosDtoAsync(int autorId);
        Task<Autor?> GetAutorAsNoTrackingAsync(int autorId);
        Task<IEnumerable<AutorDTO>> GetAutoresFiltro(AutorFiltroDTO autorFiltroDTO);
        Task CreateAutorAsync(AutorCreacionDTO autorCreacionDto);
        Task UpdateAutorAsync(AutorPutDTO autorPutDTO);
        AutorPatchDTO HandleAutorPatchDtoMapping(Autor autor);
        AutorPutDTO HandleAutorPutDtoMapping(int autorId,AutorPatchDTO autorPatchDto);
        Task<bool> DeleteAutorAsync(int autorId);
    }
}
