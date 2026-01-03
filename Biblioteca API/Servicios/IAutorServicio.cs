using Biblioteca_API.DTOs;

namespace Biblioteca_API.Servicios
{
    public interface IAutorServicio
    {
        Task<IEnumerable<AutorDTO>> GetAutoresDtoAsync();
        Task<AutorDTO?> GetAutorDtoAsync(int autorId);
        Task<AutorSinLibrosDTO?> GetAutorSinLibrosDtoAsync(int autorId);
        Task CreateAutorAsync(AutorCreacionDTO autorCreacionDto);
        Task UpdateAutorAsync(int autodIdFromRoute,AutorPutDTO autorPutDTO);
        Task<int> DeleteAutorAsyc(int autorId);
    }
}
