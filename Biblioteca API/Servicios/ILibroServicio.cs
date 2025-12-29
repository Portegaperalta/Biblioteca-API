using Biblioteca_API.DTOs;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task<IEnumerable<LibroDTO>> MapLibrosToDto();
        Task MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCracionDto);
    }
}
