using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca_API.Controllers.V1;
using Biblioteca_API.DTOs;
using Biblioteca_API.Entidades;
using Biblioteca_API.Servicios;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace BibliotecaAPITests.PruebasUnitarias.Controllers.V1
{
    [TestClass]
    public class UsuariosControllerPruebas
    {
        private UserManager<Usuario> userManager = null!;
        private SignInManager<Usuario> signInManager = null!;
        private IUsuarioServicio usuarioServicio = null!;
        private Microsoft.Extensions.Configuration.IConfiguration configuration = null!;
        private UsuariosController usuariosController = null!;

        [TestInitialize]
        public void Setup()
        {
            var userStore = Substitute.For<IUserStore<Usuario>>();
            userManager = Substitute.For<UserManager<Usuario>>(
                userStore,
                null, null, null, null, null, null, null, null
                );

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            var userClaimsPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<Usuario>>();

            signInManager = Substitute.For<SignInManager<Usuario>>(
                userManager,
                httpContextAccessor,
                userClaimsPrincipalFactory,
                null,null,null,null
                );


            usuarioServicio = Substitute.For<IUsuarioServicio>();
            configuration = Substitute.For<Microsoft.Extensions.Configuration.IConfiguration>();
            usuariosController = new UsuariosController(
                userManager,
                configuration,
                signInManager,
                usuarioServicio);
        }

        [TestMethod]
        public async Task Registrar_RetornaValidationProblem_CuandoNoEsExitoso()
        {
            //Preparacion
            var mensajeDeError = "prueba";
            var credencialesUsuarioDTO = new CredencialesUsuarioDTO
            { 
                Email = "example@hotmail.com",
                Password = "@Example123"
            };

            userManager.CreateAsync(Arg.Any<Usuario>(), Arg.Any<string>())
                       .Returns(IdentityResult.Failed(new IdentityError
                       {
                       Code = "prueba",
                       Description = "prueba"
                       }));


            //Prueba
            var respuesta = await usuariosController.Registrar(credencialesUsuarioDTO);

            //Validacion
            var resultado = respuesta.Result as ObjectResult;
            var problemDetails = resultado!.Value as ValidationProblemDetails;
            Assert.IsNotNull(problemDetails);
            Assert.AreEqual(expected: 1, actual: problemDetails.Errors.Keys.Count);
            Assert.AreEqual(expected: mensajeDeError, actual: problemDetails.Errors.Values.First().First());
        }

        [TestMethod]
        public async Task Registrar_RetornaToken_CuandoEsExitoso()
        {
            //Preparacion
            var credencialesUsuarioDTO = new CredencialesUsuarioDTO
            {
                Email = "example@hotmail.com",
                Password = "@Example123"
            };

            userManager.CreateAsync(Arg.Any<Usuario>(), Arg.Any<string>())
                       .Returns(IdentityResult.Success);

            //Prueba 
            var respuesta = await usuariosController.Registrar(credencialesUsuarioDTO);

            //Validacion
            Assert.IsNotNull(respuesta.Value);
            Assert.IsNotNull(respuesta.Value.Token);
        }

        [TestMethod]
        public async Task Login_RetornaValidationProblem_CuandoUsuarioNoExiste()
        {
            //Preparacion
            var credencialesUsuarioDTO = new CredencialesUsuarioDTO
            {
                Email = "example@hotmail.com",
                Password = "@Example123"
            };

            userManager.FindByEmailAsync(credencialesUsuarioDTO.Email)!
                       .Returns(Task.FromResult<Usuario>(null!));

            //Prueba
            var respuesta = await usuariosController.Login(credencialesUsuarioDTO);

            //Validacion
            var resultado = respuesta.Result as ObjectResult;
            var problemDetails = resultado!.Value as ValidationProblemDetails;
            Assert.IsNotNull(problemDetails);
            Assert.AreEqual(expected: 1, actual: problemDetails.Errors.Keys.Count);
            Assert.AreEqual(expected: "Login incorrecto", 
                            actual: problemDetails.Errors.Values.First().First());
        }
    }
}
