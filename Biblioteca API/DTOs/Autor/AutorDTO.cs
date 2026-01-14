using Biblioteca_API.DTOs.HATEOAS;
using Biblioteca_API.DTOs.Libro;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs.Autor
{
    public class AutorDTO : RecursoDTO
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        public string? Foto { get; set; }
        public IEnumerable<LibroDTO> Libros { get; set; } = [];
    }
}
