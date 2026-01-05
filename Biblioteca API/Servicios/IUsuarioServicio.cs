using Microsoft.AspNetCore.Identity;

namespace Biblioteca_API.Servicios
{
    public interface IUsuarioServicio
    {
        Task<IdentityUser?> ObtenerUsuario();
    }
}
