using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
