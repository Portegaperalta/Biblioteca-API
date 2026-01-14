using System.Threading.Tasks;
using Biblioteca_API.DTOs.HATEOAS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca_API.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet("ObtenerRootV1",Name = "ObtenerRootV1")]
        [AllowAnonymous]
        public async Task<IEnumerable<DatosHATEOASDTO>> Get()
        {
            var datosHateoas = new List<DatosHATEOASDTO>();

            var esAdmin = await _authorizationService.AuthorizeAsync(User, "esAdmin");

            //Acciones que cualquiera puede realizar

            datosHateoas.Add(new DatosHATEOASDTO(
                                 Enlace: Url.Link("ObtenerRootV1", new {})!,
                                 Descripcion: "self",
                                 Metodo:"GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerAutoresV1",new {})!,
                Descripcion:"autores-obtener",
                Metodo:"GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerLibrosV1", new { })!,
                Descripcion: "libros-obtener",
                Metodo: "GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerComentariosV1", new {libroId = 1 })!,
                Descripcion: "comentarios-obtener",
                Metodo: "GET"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("RegistrarUsuarioV1", new { })!,
                Descripcion: "usuario-registrar",
                Metodo: "POST"));

            datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("LoginUsuarioV1", new { })!,
                Descripcion: "usuario-login",
                Metodo: "POST"));

            //Acciones que solo usuarios autenticados pueden realizar
            if (User.Identity!.IsAuthenticated)
            {
                datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("CrearComentarioV1", new { })!,
                Descripcion: "comentario-crear",
                Metodo: "POST"));

                datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("RenovarTokenV1", new { })!,
                Descripcion: "token-renovar",
                Metodo: "GET"));
            }

            //Acciones que solo usuarios admin puede realizar

            if (esAdmin.Succeeded)
            {
                datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("CrearAutorV1", new { })!,
                Descripcion: "autor-crear",
                Metodo: "POST"));

                datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("CrearLibroV1", new { })!,
                Descripcion: "libro-crear",
                Metodo: "POST"));

                datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("ObtenerUsuariosV1", new { })!,
                Descripcion: "usuarios-obtener",
                Metodo: "GET"));

                datosHateoas.Add(new DatosHATEOASDTO(
                Enlace: Url.Link("HacerAdminV1", new { })!,
                Descripcion: "admin-hacer",
                Metodo: "POST"));
            }

            return datosHateoas;
        }
    }
}
