using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [StringLength(30,ErrorMessage ="El campo {0} debe tener {1} caracteres o menos")]
        public required string Nombre { get; set; }
        public List<Libro> Libros { get; set; } = new List<Libro>();

        [Range(18,120)]
        public int Edad { get; set; }
        }
}
