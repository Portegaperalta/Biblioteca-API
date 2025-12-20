using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        public IEnumerable<LibroDTO> Libros { get; set; } = [];
    }
}
