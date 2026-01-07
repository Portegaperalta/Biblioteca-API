using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;

namespace Biblioteca_API.Mappers
{
    public class UsuarioMapper
    {
        public UsuarioDTO MapToDto(Usuario usuario)
        {
            return new UsuarioDTO
            {
             Email = usuario.Email!,
             FechaNacimiento = usuario.FechaNacimiento
            };
        }
    }
}
