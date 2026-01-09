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
                Foto = autor.Foto,
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
                Foto = autorCreacionDto.Foto
            };
        }

        public Autor MapAutorPutDtoToAutor(AutorPutDTO autorPutDto)
        {
            return new Autor
            {
                 Id = autorPutDto.Id,
                 Nombres = autorPutDto.Nombres,
                 Apellidos = autorPutDto.Apellidos,
                 Identificacion = autorPutDto.Identificacion,
                 Foto = autorPutDto.Foto
            };
        }

        public AutorSinLibrosDTO MapToAutorSinLibrosDto(Autor autor)
        {
            return new AutorSinLibrosDTO
            {
             Id = autor.Id,
             NombreCompleto = $"{autor.Nombres} {autor.Apellidos}",
             Foto = autor.Foto
            };
        }

        public AutorPatchDTO MapToAutorPatchDto(Autor autor)
        {
            return new AutorPatchDTO
            {
             Nombres = autor.Nombres,
             Apellidos = autor.Apellidos,
             Identificacion = autor.Identificacion,
             Foto = autor.Foto
            };
        }

        public AutorPutDTO MapPatchDtoToPutDto(int autorId,AutorPatchDTO autorPatchDto)
        {
            return new AutorPutDTO
            {
             Id = autorId,
             Nombres = autorPatchDto.Nombres,
             Apellidos = autorPatchDto.Apellidos,
             Identificacion = autorPatchDto.Identificacion,
             Foto = autorPatchDto.Foto
            };
        }
    }
}
