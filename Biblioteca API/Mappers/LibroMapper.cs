using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Mappers
{
    public class LibroMapper
    {
        public LibroDTO? MapToDto(Libro libro)
        {
            if (libro is null)
            {
                return null;
            }

            return new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo
            };
        }

        public Libro? MapToEntity(LibroCreacionDTO libroCreacionDto)
        {
            if (libroCreacionDto is null)
            {
                return null;
            }

            return new Libro
            {
                Titulo = libroCreacionDto.Titulo,
                Autores = libroCreacionDto.AutoresIds
               .Select(id => new AutorLibro { AutorId = id }).ToList()
            };
        }
    }
}
