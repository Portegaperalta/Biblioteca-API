using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task CreateLibroAsync(LibroCreacionDTO libro);
        Task<IEnumerable<LibroDTO>> MapLibrosToDto();
        Task<Libro> MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCracionDto);
    }
}
