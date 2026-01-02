using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public interface ILibroServicio
    {
        Task <IEnumerable<LibroDTO>> GetLibrosDtoAsync();
        Task<Libro?> GetLibroAsync(int libroId);
        Task<LibroDTO?> GetLibroDtoAsync(int libroId);
        Task<LibroConAutoresDTO> GetLibroConAutoresDto(int libroId);
        Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto);
        Task UpdateLibroAsync(int libroId,Libro libro);
        Task<int> DeleteLibroAsync(int libroId);
    }
}
