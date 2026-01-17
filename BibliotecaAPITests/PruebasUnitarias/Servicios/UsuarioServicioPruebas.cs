using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Biblioteca_API.Datos;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Biblioteca_API.Servicios;
using BibliotecaAPITests.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace BibliotecaAPITests.PruebasUnitarias.Servicios
{
    [TestClass]
    public class UsuarioServicioPruebas : BasePruebas
    {
        private ApplicationDbContext context = null!;
        private UserManager<Usuario> userManager = null!;
        private IHttpContextAccessor httpContextAccessor = null!;
        private UsuarioMapper usuarioMapper = null!;
        private UsuarioServicio usuarioServicio = null!;
        private string nombreDb = new Guid().ToString();

        [TestInitialize]
        public void Setup()
        {
            context = ConstruirContext(nombreDb);
            userManager = Substitute.For<UserManager<Usuario>>(
                Substitute.For<IUserStore<Usuario>>(),null, null, null, null, null, null, null, null);
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            usuarioMapper = Substitute.For<UsuarioMapper>();
            usuarioServicio = new UsuarioServicio(context,userManager,httpContextAccessor,usuarioMapper);
        }

        [TestMethod]
        public async Task ObtenerUsuario_RetornaNull_CuandoEmailClaimEsNull()
        {
            //Preparacion
            var httpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.Returns(httpContext);

            //Prueba
            var respuesta = await usuarioServicio.ObtenerUsuario();

            //Validacion
            Assert.IsNull(respuesta);
        }

        [TestMethod]
        public async Task ObtenerUsuario_RetornaUsuario_CuandoEmailClaimExiste()
        {
            //Preparacion
            var email = "prueba@hotmail.com";
            var usuarioEsperado = new Usuario { Email = email };

            userManager.FindByEmailAsync(email)!.Returns(Task.FromResult(usuarioEsperado));

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("email",email)
            }));

            var httpContext = new DefaultHttpContext() { User = claims};
            httpContextAccessor.HttpContext.Returns(httpContext);
            //Prueba
            var usuario = await usuarioServicio.ObtenerUsuario();

            //Validacion
            Assert.IsNotNull(usuario);
            Assert.AreEqual(expected: email, actual: usuario.Email);
        }
    }
}
