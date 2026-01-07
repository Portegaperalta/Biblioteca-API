using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca_API.Servicios
{
    public interface IUsuarioServicio
    {
        Task<IEnumerable<UsuarioDTO>> GetAll();
        Task<Usuario?> ObtenerUsuario();
    }
}
