namespace Biblioteca_API.DTOs
{
    public class AutorSinLibrosDTO
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        public string? Foto { get; set; }
    }
}
