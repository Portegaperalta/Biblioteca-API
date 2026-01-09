using System.ComponentModel.DataAnnotations;
using Biblioteca_API.Validaciones;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [StringLength(150,ErrorMessage ="El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Nombres { get; set; }

        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [StringLength(150,ErrorMessage ="El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Apellidos { get; set; }

        [StringLength(20, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public string? Identificacion { get; set; }
        [Unicode(false)]
        public string? Foto { get; set; }
        public List<AutorLibro> Libros { get; set; } = [];
        }
}
