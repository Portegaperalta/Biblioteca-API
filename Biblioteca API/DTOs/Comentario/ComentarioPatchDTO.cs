using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.DTOs.Comentario
{
    public class ComentarioPatchDTO
    {
        [StringLength(500, ErrorMessage = "El campo {0} solo acepta {1} caracteres o menos")]
        public required string Cuerpo { get; set; }
    }
}
