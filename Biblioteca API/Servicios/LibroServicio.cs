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

        public async Task<IEnumerable<LibroDTO>> GetLibrosDtoAsync()
        {
            var libros = await _repositorioLibro.GetLibrosAsync();
            var librosDto = MapLibrosToDto(libros);
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

            var libroDto = MapLibroToDto(libro);
            return libroDto;
        }

        public async Task<LibroConAutoresDTO> GetLibroConAutoresDto(int libroId)
        {
            var libro = await _repositorioLibro.GetLibroAsync(libroId);
            var libroConAutoresDto = MapLibroToLibroConAutoresDto(libro);
            return libroConAutoresDto;
        }

        public async Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto)
        {
            if (libroCreacionDto.AutoresIds is null || libroCreacionDto.AutoresIds.Count == 0)
            {
                throw new ArgumentException("No se puede crear un libro sin autores");
            }

            bool existenAutores = await _repositorioLibro.ExistenAutores(libroCreacionDto.AutoresIds);

            if (!existenAutores)
            {
                throw new ArgumentException("Uno o mas autores no existen");
            }

            var libro = MapLibroCreacionDtoToLibro(libroCreacionDto);
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

        //Mappea entidad libro a un LibroConAutoresDTO
        public LibroConAutoresDTO MapLibroToLibroConAutoresDto(Libro libro)
        {
            var libroConAutoresDto = new LibroConAutoresDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autores = _repositorioLibro.GetLibroConAutorAsync
            };

            return libroConAutoresDto;
        }

        // Mappea lista de entidad Libro a lista de LibroDTO
        public IEnumerable<LibroDTO> MapLibrosToDto(IEnumerable<Libro> libros)
        {
            var librosDto = libros.Select(libro =>
            new LibroDTO
            {
              Id = libro.Id,
              Titulo = libro.Titulo
            });

            return librosDto;
        }

        //Mappea un LibroCreacionDTO a entidad Libro
        public Libro MapLibroCreacionDtoToLibro(LibroCreacionDTO libroCreacionDTO)
        {
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
