using System.ComponentModel.DataAnnotations;
using Biblioteca_API.DTOs.HATEOAS;

namespace Biblioteca_API.DTOs.Libro
{
    public class LibroDTO : RecursoDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "El titulo del libro debe ser 250 caracteres o menos")]
        public required string Titulo { get; set; }
    }
}
