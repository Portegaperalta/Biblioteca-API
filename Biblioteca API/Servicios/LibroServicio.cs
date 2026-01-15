using System.Threading.Tasks;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.DTOs.Libro;
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

            if (libro is null)
            {
                return null;
            }
            
            return libro;
        }

        public async Task<LibroDTO?> GetLibroDtoAsync(int libroId)
        {
            var libro = await _repositorioLibro.GetLibroAsync(libroId);

            if (libro is null)
            {
                return null;
            }

            var libroDto = _libroMapper.MapToLibroDto(libro);
            return libroDto;
        }

        public async Task<LibroConAutoresDTO?> GetLibroConAutoresDto(int libroId)
        {
            var libro = await _repositorioLibro.GetLibroConAutorAsync(libroId);

            if (libro is null)
            {
                return null;
            }

            var libroConAutoresDto = _libroMapper.MapToLibroConAutoresDto(libro);
            return libroConAutoresDto;
        }

        public async Task<IEnumerable<LibroDTO>> GetLibrosDtoAsync(PaginacionDTO paginacionDTO)
        {
            var libros = await _repositorioLibro.GetLibrosAsync(paginacionDTO);
            var librosDto = libros.Select(libro => _libroMapper.MapToLibroDto(libro));
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

        public async Task UpdateLibroAsync(int libroIdFromRoute,LibroPutDTO libroPutDto)
        {
            if (libroPutDto.AutoresIds is null || libroPutDto.AutoresIds.Count == 0)
            {
                throw new ArgumentException("El libro debe tener un autor o mas");
            }

            var autoresIdExistentes = await _repositorioLibro.GetLibroAutoresId(libroPutDto);

            if (autoresIdExistentes.Count() != libroPutDto.AutoresIds.Count)
            {
                var autoresIdNoExistentes = libroPutDto.AutoresIds.Except(autoresIdExistentes);
                var autoresNoExistentesString = string.Join(",", autoresIdNoExistentes);

                throw new ArgumentException($"Los siguientes autores Ids no existen: {autoresNoExistentesString}");
            }

            var libro = _libroMapper.MapLibroPutDtoToLibro(libroPutDto);

            await _repositorioLibro.UpdateLibroAsync(libro);

            // Elimina relacion existente en tabla intermedia AutoresLibros
            // para evitar records duplicados y vuelve a insertar relacion 
            // con nueva lista de autoresIds
            await _repositorioLibro.DeleteAutoresLibrosAsync(libroIdFromRoute);
            await _repositorioLibro.InsertAutoresLibrosAsync(libroIdFromRoute, libroPutDto.AutoresIds);
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
