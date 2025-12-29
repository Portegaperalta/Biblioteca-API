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

        public async Task<IEnumerable<LibroDTO>> GetLibrosAsync()
        {
            var librosDto = await MapLibrosToDto();
            return librosDto;
        }

        public async Task<Libro?> GetLibroAsync(int libroId)
        {
            var libro = await _repositorioLibro.GetLibroAsync(libroId);
            return libro;
        }

        public async Task<LibroDTO> GetLibroDtoAsync(int libroId)
        {
            var libro = await _repositorioLibro.GetLibroAsync(libroId);

            if (libro is null)
            {
                return null;
            }

            var libroDto = await MapLibroToDto(libro);
            return libroDto;
        }

        public async Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto)
        {
            var libro = await MapLibroCreacionDtoToLibro(libroCreacionDto);
            await _repositorioLibro.CreateLibroAsync(libro);
        }

        public async Task<int> DeleteLibroAsync(int libroId)
        {
            var registrosBorrados = await _repositorioLibro.DeleteLibroAsync(libroId);
            return registrosBorrados;
        }

        //Mappea entidad libro a un LibroDTO
        public LibroDTO MapLibroToDto (Libro libro)
        {
            var libroDto = new LibroDTO 
            { 
                Id = libro.Id,
                Titulo = libro.Titulo
            };

            return libroDto;
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
        public async Task<Libro> MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds is null || libroCreacionDTO.AutoresIds.Count == 0)
            {
                throw new ArgumentException("No se puede crear un libro sin autores");
            }

            bool existenAutores = await _repositorioLibro.ExistenAutores(libroCreacionDTO.AutoresIds);

            if (!existenAutores)
            {
                throw new ArgumentException("Uno o mas autores no existen");
            }

            var libro = new Libro
            {
                Titulo = libroCreacionDTO.Titulo,
                Autores = libroCreacionDTO.AutoresIds.Select(id =>
                          new AutorLibro { AutorId = id }).ToList()
            };

            return libro;
        }
    }
}
