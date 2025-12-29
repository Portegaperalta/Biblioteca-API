using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task<IEnumerable<LibroDTO>> MapLibrosToDto();
        Task<Libro> MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCracionDto);
    }
}
