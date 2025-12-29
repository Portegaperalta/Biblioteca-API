using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Servicios
{
    public class LibroServicio : ILibroServicio
    {
        private readonly IRepositorioLibro _repositorioLibro;

        public LibroServicio(IRepositorioLibro repositorioLibro)
        {
            _repositorioLibro = repositorioLibro;
        }

        // Mappea lista de entidad Libro a lista de LibroDTO
        public async Task<IEnumerable<LibroDTO>> MapLibrosToDto()
        {
            var libros = await _repositorioLibro.GetLibrosAsync();
            var librosDto = libros.Select(libro =>
            new LibroDTO
            {
              Id = libro.Id,
              Titulo = libro.Titulo
            });

            return librosDto;
        }

        //Mappea un LibroCreacionDTO a entidad Libro
        public async Task MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCreacionDTO)
        {
            var libro = new Libro
            {
                Titulo = libroCreacionDTO.Titulo,
                Autores = libroCreacionDTO.AutoresIds.Select(id => new AutorLibro { AutorId = id }).ToList()
            };

            await _repositorioLibro.CreateLibroAsync(libro);
        }
    }
}
