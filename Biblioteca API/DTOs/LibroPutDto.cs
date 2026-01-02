using System.ComponentModel.DataAnnotations;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.DTOs
{
    public class LibroPutDTO
    {
        public required string Titulo { get; set; }
        [Required]
        public List<AutorLibro> Autores { get; set; } = [];
    }
}
