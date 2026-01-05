using Microsoft.AspNetCore.Identity;

namespace Biblioteca_API.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioServicio
            (UserManager<IdentityUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityUser?> ObtenerUsuario()
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
