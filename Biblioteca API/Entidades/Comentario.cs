namespace Biblioteca_API.Entidades
{
    public class Comentario
    {
        public Guid Id { get; set; }
        public required string Autor { get; set; }
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }
    }
}
