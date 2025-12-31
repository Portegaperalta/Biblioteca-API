using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;

namespace Biblioteca_API.Servicios
{
    public class LibroServicio : ILibroServicio
    {
        private readonly IRepositorioLibro _repositorioLibro;
        private readonly LibroMapper _libroMapper;

        public LibroServicio(IRepositorioLibro repositorioLibro,LibroMapper libroMapper)
        {
            _repositorioLibro = repositorioLibro;
            _libroMapper = libroMapper;
        }

        public async Task<IEnumerable<LibroDTO>> GetLibrosDtoAsync()
        {
            var libros = await _repositorioLibro.GetLibrosAsync();
            var librosDto = libros.Select(libro => _libroMapper.MapToDto(libro));
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

            var libroDto = _libroMapper.MapToDto(libro);
            return libroDto;
        }

        public async Task<LibroConAutoresDTO?> GetLibroConAutoresDto(int libroId)
        {
            var libroConAutores = await _repositorioLibro.GetLibroConAutorAsync(libroId);

            if (libroConAutores is null)
            {
                return null;
            }

            var libroConAutoresDto = _libroMapper.MapToLibroConAutoresDto(libroConAutores);
            return libroConAutoresDto;
        }

        public async Task CreateLibroAsync(LibroCreacionDTO libroCreacionDto)
        {
            if (libroCreacionDto.AutoresIds is null || libroCreacionDto.AutoresIds.Count == 0)
            {
                throw new ArgumentException("No se puede crear un libro sin autores");
            }

            var autoresIdExistentes = await _repositorioLibro.GetLibroAutoresId(libroCreacionDto);

            if (autoresIdExistentes.Count() != libroCreacionDto.AutoresIds.Count)
            {
                throw new ArgumentException("Uno o mas autores no existen");
            }

            var libro = _libroMapper.MapToEntity(libroCreacionDto);
            await _repositorioLibro.CreateLibroAsync(libro);
        }

        public async Task<int> DeleteLibroAsync(int libroId)
        {
            var registrosBorrados = await _repositorioLibro.DeleteLibroAsync(libroId);
            return registrosBorrados;
        }
    }
}
