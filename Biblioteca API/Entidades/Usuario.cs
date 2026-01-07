using Microsoft.AspNetCore.Identity;

namespace Biblioteca_API.Entidades
{
    public class Usuario : IdentityUser
    {
        public DateTime FechaNacimiento { get; set; }
    }
}
