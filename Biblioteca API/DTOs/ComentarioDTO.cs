using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.DTOs
{
    public class ComentarioDTO
    {
        public Guid Id { get; set; }

        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Autor { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} solo acepta {1} caracteres o menos")]
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow.Date;
        public required string AutorId { get; set; }
        public required string AutorEmail { get; set; }
    }
}
