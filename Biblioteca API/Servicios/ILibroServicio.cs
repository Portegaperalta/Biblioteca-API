using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        List<Libro> GetLibrosDtoAsync(IEnumerable<Libro> libros);
        Task<Libro?> GetLibroAsync(int libroId);
        Task<LibroDTO> GetLibroDtoAsync(int libroId);
        Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto);
        Task<int> DeleteLibroAsync(int libroId);
        Task<IEnumerable<LibroDTO>> MapLibrosToDto();
        LibroDTO MapLibroToDto(Libro libro);
        Task<Libro> MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCracionDto);
    }
}
