using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioAutor
    {
        Task<IEnumerable<Autor>> GetAutoresAsync(PaginacionDTO paginacionDTO);
        Task<Autor?> GetAutorAsync(int id);
        Task<Autor?> GetAutorAsNoTrackingAsync(int id);
        Task<Autor?> GetAutorSinLibrosAsync(int id);
        Task<string?> GetFotoActualAutor(int id);
        Task CreateAutorAsync(Autor autor);
        Task UpdateAutorAsync(Autor autor);
        Task DeleteAutorAsync(Autor autor);
    }
}
