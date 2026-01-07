using Biblioteca_API.Datos;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_API.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioMapper _usuarioMapper;

        public UsuarioServicio
            (ApplicationDbContext context,UserManager<Usuario> userManager,IHttpContextAccessor httpContextAccessor, UsuarioMapper usuarioMapper)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _usuarioMapper = usuarioMapper;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAll()
        {
            var usuariosDb = await _context.Users.ToListAsync();
            var usuariosDtos = usuariosDb.Select(usuarioDb => 
                              _usuarioMapper.MapToDto(usuarioDb));

            return usuariosDtos;
        }

        public async Task<Usuario?> ObtenerUsuario()
        {
            var emailClaim = _httpContextAccessor.HttpContext!
                                                 .User.Claims
                                                 .Where(c => c.Type == "email")
                                                 .FirstOrDefault();

            if (emailClaim is null)
            {
                return null;
            }

            var email = emailClaim.Value;
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
