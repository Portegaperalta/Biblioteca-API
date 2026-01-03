using Biblioteca_API.Datos.Repositorios;

namespace Biblioteca_API.Servicios
{
    public class AutorServicio : IAutorServicio
    {
        private readonly IRepositorioAutor _repositorioAutor;

        public AutorServicio(IRepositorioAutor repositorioAutor)
        {
            _repositorioAutor = repositorioAutor;
        }
    }
}
