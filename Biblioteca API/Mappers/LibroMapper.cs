using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Mappers
{
    public class LibroMapper
    {
        public LibroDTO MapToDto(Libro libro)
        {
            return new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo
            };
        }

        public Libro MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCreacionDto)
        {
            return new Libro
            {
                Titulo = libroCreacionDto.Titulo,
                Autores = libroCreacionDto.AutoresIds
               .Select(id => new AutorLibro { AutorId = id }).ToList()
            };
        }

        public LibroConAutoresDTO MapToLibroConAutoresDto (Libro libro)
        {
            return new LibroConAutoresDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autores = libro.Autores.Select(autores => new AutorDTO
                { 
                  Id = autores.Autor.Id,
                  NombreCompleto = $"{autores.Autor.Nombres} {autores.Autor.Apellidos}",
                }).ToList()
            };
        }
    }
}
