using System.ComponentModel.DataAnnotations;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs
{
    public class LibroPutDTO
    {
        [Required]
        [StringLength(250, ErrorMessage = "El titulo del libro debe ser 250 caracteres o menos")]
        public required string Titulo { get; set; }

        [Required]
        public List<int> AutoresIds { get; set; } = [];
    }
}
