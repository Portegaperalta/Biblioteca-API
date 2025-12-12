using System.ComponentModel.DataAnnotations;

namespace Biblioteca_API.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
    }
}
