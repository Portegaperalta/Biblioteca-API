using Biblioteca_API.DTOs.Autor;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs.Libro
{
    public class LibroConAutoresDTO : LibroDTO
    {
        public required List<AutorSinLibrosDTO> Autores { get; set; } = [];
    }
}
