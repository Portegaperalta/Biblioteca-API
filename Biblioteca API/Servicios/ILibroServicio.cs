using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task <IEnumerable<LibroDTO>> GetLibrosDtoAsync();
        Task<Libro?> GetLibroAsync(int libroId);
        Task<LibroDTO> GetLibroDtoAsync(int libroId);
        Task<LibroConAutoresDTO?> GetLibroConAutoresDto(int libroId);
        Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto);
        Task<int> DeleteLibroAsync(int libroId);
        IEnumerable<LibroDTO> MapLibrosToDto(IEnumerable<Libro> libros);
        LibroDTO MapLibroToDto(Libro libro);
        LibroConAutoresDTO MapLibroToLibroConAutoresDto(Libro libro);
    }
}
