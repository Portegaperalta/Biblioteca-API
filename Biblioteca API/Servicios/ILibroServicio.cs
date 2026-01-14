using Biblioteca_API.DTOs;
using Biblioteca_API.DTOs.Libro;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task <IEnumerable<LibroDTO>> GetLibrosDtoAsync(PaginacionDTO paginacionDTO);
        Task<Libro?> GetLibroAsync(int libroId);
        Task<LibroDTO?> GetLibroDtoAsync(int libroId);
        Task<LibroConAutoresDTO?> GetLibroConAutoresDto(int libroId);
        Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto);
        Task UpdateLibroAsync(int libroId,LibroPutDTO libroPutDto);
        Task<int> DeleteLibroAsync(int libroId);
    }
}
