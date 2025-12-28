using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.DTOs
{
    public class LibroCreacionDTO
    {
        [Required]
        [StringLength(250, ErrorMessage = "El titulo del libro debe ser 250 caracteres o menos")]
        public required string Titulo { get; set; }
        [Required]
        public required List<int> AutoresIds = [];
    }
}
