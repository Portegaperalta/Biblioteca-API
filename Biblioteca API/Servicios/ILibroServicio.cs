using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task<IEnumerable<LibroDTO>> GetLibrosAsync();
        Task<LibroDTO> GetLibroAsync(int libroId);
        Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto);
        Task<int> DeleteLibroAsync(int libroId);
        Task<IEnumerable<LibroDTO>> MapLibrosToDto();
        Task<Libro> MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCracionDto);
    }
}
