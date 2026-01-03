using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Mappers
{
    public class LibroMapper
    {
        public LibroDTO MapToLibroDto(Libro libro)
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
                Autores = libro.Autores.Select(autoresLibros => new AutorSinLibrosDTO
                {
                 Id = autoresLibros.AutorId,
                 NombreCompleto = $"{autoresLibros.Autor.Nombres} {autoresLibros.Autor.Apellidos}"
                }).ToList()
            };
        }

        public Libro MapLibroPutDtoToLibro(LibroPutDTO libroPutDto)
        {
            return new Libro
            {
                Id = libroPutDto.Id,
                Titulo = libroPutDto.Titulo
            };
        }
    }
}
