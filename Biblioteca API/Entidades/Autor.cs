using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo Nombre es obligatorio")]
        public required string Nombre { get; set; }
        public List<Libro> Libros { get; set; } = new List<Libro>();
    }
}
