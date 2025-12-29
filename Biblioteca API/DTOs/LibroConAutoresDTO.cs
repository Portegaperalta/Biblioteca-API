using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs
{
    public class LibroConAutoresDTO : LibroDTO
    {
        public required List<Autor> Autores { get; set; } = [];
    }
}
