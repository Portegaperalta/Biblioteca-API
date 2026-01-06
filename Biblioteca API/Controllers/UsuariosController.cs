using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Biblioteca_API.DTOs;
using Biblioteca_API.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Biblioteca_API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IConfiguration _configuration;

        public UsuariosController
            (
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            IUsuarioServicio usuarioServicio
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _usuarioServicio = usuarioServicio;
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDto>> Registrar
            (CredencialesUsuarioDTO credencialesUsuario)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email,
            };

            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password!);
        
            if (resultado.Succeeded)
            {
                var respuestaAutenticacion = await ConstruirToken(credencialesUsuario);
                return respuestaAutenticacion;
            } else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return ValidationProblem();
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDto>> Login
            (CredencialesUsuarioDTO credencialesUsuario)
        {
            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

            if (usuario is null)
            {
                return RetornarLoginIncorrecto();
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, credencialesUsuario.Password!, false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            } else
            {
                return RetornarLoginIncorrecto();
            }
        }

        [HttpGet("renovar-token")]
        public async Task<ActionResult<RespuestaAutenticacionDto>> RenovarToken()
        {
            var usuario = await _usuarioServicio.ObtenerUsuario();

            if (usuario is null)
                return NotFound();

            var credencialesUsuarioDto = new CredencialesUsuarioDTO
                                         {
                                            Email = usuario.Email!,
                                         };

            var respuestaAutenticacion = await ConstruirToken(credencialesUsuarioDto);

            return respuestaAutenticacion;
        }

        private ActionResult RetornarLoginIncorrecto()
        {
            ModelState.AddModelError(string.Empty, "Login incorrecto");
            return ValidationProblem();
        }

        private async Task<RespuestaAutenticacionDto> ConstruirToken
            (CredencialesUsuarioDTO credencialesUsuarioDto)
        {
            var claims = new List<Claim>
            {
               new Claim("email",credencialesUsuarioDto.Email),
            };

            var usuario = await _userManager.FindByEmailAsync(credencialesUsuarioDto.Email);
            var claimsDb = await _userManager.GetClaimsAsync(usuario!);

            claims.AddRange(claimsDb);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenDeSeguridad = new JwtSecurityToken 
                (issuer:null,audience:null,claims,expires:expiracion,signingCredentials:credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDto
            {
             Token = token,
             Expiracion = expiracion
            };
        }
    }
}
