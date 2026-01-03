using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
using Biblioteca_API.Mappers;

namespace Biblioteca_API.Servicios
{
    public class AutorServicio : IAutorServicio
    {
        private readonly IRepositorioAutor _repositorioAutor;
        private readonly AutorMapper _autorMapper;

        public AutorServicio(IRepositorioAutor repositorioAutor,AutorMapper autorMapper)
        {
            _repositorioAutor = repositorioAutor;
            _autorMapper = autorMapper;
        }

        public async Task<IEnumerable<AutorDTO>> GetAutoresDto()
        {
            var autores = await _repositorioAutor.GetAutoresAsync();
            var autoresDto = autores.Select(autor => _autorMapper.MapToAutorDto(autor));
            return autoresDto;
        }
    }
}
