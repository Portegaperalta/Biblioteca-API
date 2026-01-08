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
        private readonly ITimeLimitedDataProtector _protectorLimitadoPorTiempo;

        public SeguridadController(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector("SeguridadController");
            _protectorLimitadoPorTiempo = _protector.ToTimeLimitedDataProtector();
        }

        [HttpGet("encriptar-limitado-por-tiempo")]
        public ActionResult EncriptarLimitado([FromQuery] string textoPlano)
        {
            string textoCifrado = _protectorLimitadoPorTiempo
                                  .Protect(textoPlano,lifetime: TimeSpan.FromSeconds(30));
            return Ok(new { textoCifrado });
        }

        [HttpGet("desencriptar-limitado-por-tiempo")]
        public ActionResult DesencriptarLimitado(string textoCifrado)
        {
            string textoPlano = _protectorLimitadoPorTiempo.Unprotect(textoCifrado);
            return Ok(new { textoPlano });
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
