using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca_API.Servicios
{
    public interface IUsuarioServicio
    {
        Task<Usuario?> ObtenerUsuario();
    }
}
