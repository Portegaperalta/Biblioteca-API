using Biblioteca_API.Datos.Repositorios;
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
    }
}
