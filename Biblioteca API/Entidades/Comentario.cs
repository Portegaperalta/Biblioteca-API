using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca_API.Entidades
{
    public class Comentario
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Autor { get; set; }
        
        [StringLength(500,ErrorMessage = "El campo {0} solo acepta {1} caracteres o menos")]
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow.Date;
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }
        public required string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
