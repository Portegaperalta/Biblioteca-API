using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250,ErrorMessage ="El titulo del libro debe ser 250 caracteres o menos")]
        public required string Titulo { get; set; }
        [Required]
        public List<AutorLibro> Autores { get; set; } = [];
        public List<Comentario> Comentarios { get; set; } = [];
    }
}
