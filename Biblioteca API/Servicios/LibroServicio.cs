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

        public async Task<LibroConAutoresDTO> GetLibroConAutoresDto(int libroId)
        {
            var libroConAutores = await _repositorioLibro.GetLibroConAutorAsync(libroId);

            var libroConAutoresDto = _libroMapper.MapToLibroConAutoresDto(libroConAutores);
            return libroConAutoresDto;
        }

        public async Task<IEnumerable<LibroDTO>> GetLibrosDtoAsync()
        {
            var libros = await _repositorioLibro.GetLibrosAsync();
            var librosDto = libros.Select(libro => _libroMapper.MapToDto(libro));
            return librosDto;
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
                var autoresIdNoExistentes = libroCreacionDto.AutoresIds.Except(autoresIdExistentes);
                var autoresNoExistentesString = string.Join(",",autoresIdNoExistentes);

                throw new ArgumentException($"Los siguientes autores Id no existen: {autoresNoExistentesString}");
            }

            var libro = _libroMapper.MapLibroCreacionDtoToLibro(libroCreacionDto);
            AsignarOrdenAutores(libro);

            await _repositorioLibro.CreateLibroAsync(libro);
        }

        public async Task UpdateLibroAsync(int libroIdFromRoute,Libro libro)
        {
            if (libroIdFromRoute != libro.Id)
            {
                throw new ArgumentException($"Los ids deben de coincidir");
            }
            //agregar validadcion faltante en caso que autoresIds no existan

            await _repositorioLibro.UpdateLibroAsync(libro);
        }

        public async Task<int> DeleteLibroAsync(int libroId)
        {
            var registrosBorrados = await _repositorioLibro.DeleteLibroAsync(libroId);
            return registrosBorrados;
        }

        private void AsignarOrdenAutores (Libro libro)
        {
            if (libro.Autores != null)
            {
                for (int i = 0; i < libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden = i;
                }
            }
        }
    }
}
