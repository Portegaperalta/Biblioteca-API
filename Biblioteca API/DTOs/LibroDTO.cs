using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "El titulo del libro debe ser 250 caracteres o menos")]
        public required string Titulo { get; set; }
        
        [StringLength(250, ErrorMessage = "El nombre del autor debe ser 150 caracteres o menos")]
        public required string NombreAutor { get; set; }
    }
}
