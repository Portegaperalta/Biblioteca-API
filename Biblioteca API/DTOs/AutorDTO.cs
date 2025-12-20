using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        public List<Libro> Libros { get; set; } = new List<Libro>();
    }
}
