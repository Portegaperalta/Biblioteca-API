using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioLibro
    {
        Task<IEnumerable<Libro>> GetLibrosAsync();
        Task<Libro?> GetLibroAsync(int id);
        Task<Libro?> GetLibroConAutorAsync(int libroId);
        Task CreateLibroAsync(Libro libro);
        Task UpdateLibroAsync(Libro libro);
        Task<int> DeleteLibroAsync(int id);
        Task<bool> ExistenAutores(List<int> autoresIds);
        Task<IEnumerable<Autor>> GetLibroAutores(int libroId);
    }
}
