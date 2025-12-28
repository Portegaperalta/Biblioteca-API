using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Datos.Repositorios
{
    public interface IRepositorioLibro
    {
        Task<IEnumerable<Libro>> GetLibrosAsync();
        Task<Libro?> GetLibroAsync(int id);
        Task CreateLibroAsync(LibroCreacionDTO libroCreacionDTO);
        Task UpdateLibroAsync(Libro libro);
        Task<int> DeleteLibroAsync(int id);
        Task<bool> ExisteAutor(int id);
        Task<List<string>> GetNombreAutores(int libroId);
    }
}
