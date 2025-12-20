using System.ComponentModel.DataAnnotations;
using Biblioteca_API.Validaciones;

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

        public List<Libro> Libros { get; set; } = new List<Libro>();
        }
}
