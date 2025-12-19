using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioAutor
    {
        Task<IEnumerable<Autor>> GetAutoresAsync();
        Task<Autor?> GetAutor(int id);
        Task<Autor> GetPrimerAutor();
        Task CreateAutor(Autor autor);
        Task UpdateAutor(Autor autor);
        Task<int> DeleteAutor(int id);
    }
}
