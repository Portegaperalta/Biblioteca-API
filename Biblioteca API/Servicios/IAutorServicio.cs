using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.JsonPatch;

namespace Biblioteca_API.Servicios
{
    public interface IAutorServicio
    {
        Task<IEnumerable<AutorDTO>> GetAutoresDtoAsync();
        Task<AutorDTO?> GetAutorDtoAsync(int autorId);
        Task<Autor?> GetAutorAsync(int autorId);
        Task<AutorSinLibrosDTO?> GetAutorSinLibrosDtoAsync(int autorId);
        Task CreateAutorAsync(AutorCreacionDTO autorCreacionDto);
        Task UpdateAutorAsync(AutorPutDTO autorPutDTO);
        AutorPatchDTO HandleAutorPatch(Autor autor);
        Task<int> DeleteAutorAsync(int autorId);
    }
}
