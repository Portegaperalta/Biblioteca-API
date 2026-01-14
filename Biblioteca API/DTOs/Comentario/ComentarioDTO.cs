using System.ComponentModel.DataAnnotations;
using Biblioteca_API.DTOs.HATEOAS;

namespace Biblioteca_API.DTOs.Comentario
{
    public class ComentarioDTO : RecursoDTO
    {
        public Guid Id { get; set; }

        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Usuario { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} solo acepta {1} caracteres o menos")]
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow.Date;
        public required string UsuarioId { get; set; }
        public required string UsuarioEmail { get; set; }
    }
}
