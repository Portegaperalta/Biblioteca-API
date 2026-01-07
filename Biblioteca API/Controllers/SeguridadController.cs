using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers
{
    [Route("api/seguridad")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
  
        private readonly IDataProtector _protector;
        public SeguridadController(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector("SeguridadController");
        }

        [HttpGet("encriptar")]
        public ActionResult Encriptar([FromQuery]string textoPlano)
        {
            string textoCifrado = _protector.Protect(textoPlano);
            return Ok(new { textoCifrado });
        }

        [HttpGet("desencriptar")]
        public ActionResult Desencriptar(string textoCifrado)
        {
            string textoPlano = _protector.Unprotect(textoCifrado);
            return Ok(new {textoPlano});
        }
    }
}
