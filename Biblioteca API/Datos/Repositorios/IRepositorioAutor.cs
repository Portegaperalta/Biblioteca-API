using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioAutor
    {
        Task<IEnumerable<Autor>> GetAutoresAsync();
        Task<Autor> GetAutor(int id);
        Task<Autor> GetPrimerAutor();
        Task CreateAutor();
        Task UpdateAutor();
        Task<int> DeleteAutor();
    }
}
