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
        Task<IEnumerable<int>> GetLibroAutoresId (LibroCreacionDTO libroCreacionDto);
        Task<IEnumerable<int>> GetLibroAutoresId(LibroPutDTO libroPutDto);
        Task<IEnumerable<Autor>> GetLibroAutores(int libroId);
        Task InsertAutoresLibrosAsync(int libroId,List<int> autoresIds);
        Task<int> DeleteAutoresLibrosAsync(int libroId);
    }
}
