using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioAutor
    {
        Task<IEnumerable<Autor>> GetAutoresAsync();
        Task<Autor?> GetAutorAsync(int id);
        Task<Autor?> GetAutorSinLibrosAsync(int id);
        Task CreateAutorAsync(Autor autor);
        Task UpdateAutorAsync(Autor autor);
        Task<int> DeleteAutorAsync(int id);
    }
}
