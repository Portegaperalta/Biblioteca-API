using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Mappers
{
    public class AutorMapper
    {
        public AutorDTO MapToAutorDto(Autor autor)
        {
            return new AutorDTO
            {
                Id = autor.Id,
                NombreCompleto = $"{autor.Nombres} {autor.Apellidos}",
                Libros = autor.Libros.Select(autoresLibros => new LibroDTO
                {
                    Id = autoresLibros.LibroId,
                    Titulo = autoresLibros.Libro.Titulo
                }).ToList() ?? new List<LibroDTO>()
            };
        }

        public Autor MapToAutor(AutorCreacionDTO autorCreacionDto)
        {
            return new Autor
            {
                Nombres = autorCreacionDto.Nombres,
                Apellidos = autorCreacionDto.Apellidos,
                Identificacion = autorCreacionDto.Identificacion,
            };
        }

        public AutorSinLibrosDTO MapToAutorSinLibrosDto(Autor autor)
        {
            return new AutorSinLibrosDTO
            {
             Id = autor.Id,
             NombreCompleto = $"{autor.Nombres} {autor.Apellidos}"
            };
        }
    }
}
