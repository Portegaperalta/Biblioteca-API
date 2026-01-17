using Biblioteca_API.Controllers.V1;
using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs.Autor;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Biblioteca_API.Servicios;
using BibliotecaAPITests.Utilidades;
using BibliotecaAPITests.Utilidades.Dobles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Extensions;

namespace BibliotecaAPITests.PruebasUnitarias.Controllers.V1
{
    [TestClass]
    public class AutoresControllerPruebas : BasePruebas
    {
        //Helpers de construccion
        protected AutorServicio ConstruirAutorServicio
            (RepositorioAutor repositorioAutor,AutorMapper autorMapper,
            IAlmacenadorArchivos almacenadorArchivos,ILogger<AutorServicio> logger)
        {
            return new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);
        }

        protected RepositorioAutor ConstruirRepositorioAutor(ApplicationDbContext context)
        {
            return new RepositorioAutor(context);
        }

        protected AutorMapper ConstruirMapper()
        {
            return new AutorMapper();
        }

        IAlmacenadorArchivos almacenadorArchivos = null!;
        ILogger<AutorServicio> logger = null!;
        IOutputCacheStore outputCacheStore = null!;
        IAutorServicio autorServicio = null!;
        private string nombreDB = Guid.NewGuid().ToString();
        private AutoresController autoresController = null!;

        [TestInitialize]
        public void Setup()
        {
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = Substitute.For<IAlmacenadorArchivos>();
            ILogger<AutorServicio> logger = Substitute.For<ILogger<AutorServicio>>();
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();

            autoresController = new AutoresController(autorServicio, outputCacheStore);
        }

        //GET
        [TestMethod]
        [DataRow(1)]
        public async Task Get_Retorna404_CuandoAutorNoExiste(int autorId)
        {
            //Prueba
            var respuesta = await autoresController.Get(autorId,true);

            //Validacion
            var resultado = respuesta.Result as StatusCodeResult;
            Assert.AreEqual(expected: 404,actual:resultado!.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Get_RetornaAutorDTO_CuandoAutorExiste(int autorId)
        {
            //Preparacion
            var context = ConstruirContext(nombreDB);
            context.Add(new Autor { Nombres = "Ernest", Apellidos = "Hemingway", });
            context.Add(new Autor { Nombres = "Pablo", Apellidos = "Neruda", });
            await context.SaveChangesAsync();

            //Prueba
            var respuesta = await autoresController.Get(autorId, true);

            //Validacion
            var resultado = respuesta.Result as OkObjectResult;
            var autorDto = resultado!.Value as AutorDTO;

            Assert.AreEqual(expected: autorId, actual: autorDto!.Id);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Get_DebeLlamarGetDelServicioAutores(int autorId)
        {
            //Preparacion
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            //Prueba
            await autoresController.Get(autorId,true);

            //Verificacion
            await autorServicio.Received(1).GetAutorDtoAsync(autorId);
        }

        //POST
        [TestMethod]
        public async Task Post_Retorna201_CuandoAutorEsCreado()
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();

            var autorServicio = ConstruirAutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            var autorCreacionDTO = new AutorCreacionDTO
            {
                Nombres = "William",
                Apellidos = "Shakespeare",
                Identificacion = "123"
            };

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            //Prueba
            var respuesta = await autoresController.Post(autorCreacionDTO);

            //Validacion
            var resultado = respuesta as CreatedResult;
            Assert.AreEqual(expected: 201, actual: resultado!.StatusCode);

            //valida que autor realmente fue creado en tabla
            var contexto2 = ConstruirContext(nombreDB);
            var cantidad = await contexto2.Autores.CountAsync();

            Assert.AreEqual(expected: 1, actual: cantidad);
        }

        [TestMethod]
        public async Task Post_DebeLlamarCreateAutorAsyncDelServicioAutores()
        {
            //Preparacion
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autorCreacionDTO = new AutorCreacionDTO
            {
                Nombres = "William",
                Apellidos = "Shakespeare",
                Identificacion = "123"
            };

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            //Prueba
            await autoresController.Post(autorCreacionDTO);

            //Validacion
            await autorServicio.Received(1).CreateAutorAsync(autorCreacionDTO);
        }

        //PUT
        [TestMethod]
        [DataRow(1)]
        public async Task Put_Retorna400_CuandoAutorIdDeRutaNoEsIgualAIdDeAutorPutDTO(int autorIdFromRoute)
        {
            //Preparacion
            var autorPutDTO = new AutorPutDTO
            {
             Id = 2,
             Nombres = "William",
             Apellidos = "Shakespeare",
             Identificacion = "456"
            };

            //Prueba
            var respuesta = await autoresController.Put(autorIdFromRoute, autorPutDTO);

            //Validacion
            var resultado = respuesta as BadRequestObjectResult;
            Assert.AreEqual(expected: 400, actual: resultado!.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Put_Retorna404_CuandoAutorNoExiste(int autorIdFromRoute)
        {
            //Preparacion
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            var autorPutDTO = new AutorPutDTO
            {
                Id = 1,
                Nombres = "William",
                Apellidos = "Shakespeare",
                Identificacion = "456"
            };

            //Prueba
            var respuesta = await autoresController.Put(autorIdFromRoute, autorPutDTO);

            //Validacion
            var resultado = respuesta as NotFoundObjectResult;
            Assert.AreEqual(expected: 404, actual: resultado!.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Put_Retorna204_CuandoAutorSeActualiza(int autorIdFromRoute)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = Substitute.For<IAlmacenadorArchivos>();
            ILogger<AutorServicio> logger = Substitute.For<ILogger<AutorServicio>>();
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            var autorPutDTO = new AutorPutDTO
            {
                Id = 1,
                Nombres = "William",
                Apellidos = "Shakespeare",
                Identificacion = "456"
            };

            autorServicio.GetAutorAsNoTrackingAsync(autorIdFromRoute)
                         .Returns(new Autor
                         {
                             Id = autorIdFromRoute,
                             Nombres = "William",
                             Apellidos = "Shakespeare",
                             Identificacion = "123"
                         });

            //Prueba
            var respuesta = await autoresController.Put(autorIdFromRoute, autorPutDTO);

            //Validacion
            var resultado = respuesta as NoContentResult;
            Assert.IsNotNull(resultado);
            Assert.AreEqual(expected: 204, actual: resultado.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Put_DebeLlamarUpdateAutorAsyncDelServicioAutor(int autorIdFromRoute)
        {
            //Preparacion
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            var autorPutDTO = new AutorPutDTO
            {
                Id = 1,
                Nombres = "William",
                Apellidos = "Shakespeare",
                Identificacion = "456"
            };

            autorServicio.GetAutorAsNoTrackingAsync(autorIdFromRoute)
                         .Returns(new Autor
                         {
                             Id = autorIdFromRoute,
                             Nombres = "William",
                             Apellidos = "Shakespeare",
                             Identificacion = "123"
                         });

            //Prueba
            await autoresController.Put(autorIdFromRoute, autorPutDTO);

            //Validacion
            await autorServicio.Received(1).UpdateAutorAsync(autorPutDTO);
        }
        //DELETE
        [TestMethod]
        [DataRow(1)]
        public async Task Delete_Retorna404_CuandoAutorNoExiste(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = Substitute.For<IAlmacenadorArchivos>();
            ILogger<AutorServicio> logger = Substitute.For<ILogger<AutorServicio>>();
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            //Prueba
            var respuesta = await autoresController.Delete(autorId);

            //Validacion
            var resultado = respuesta as NotFoundResult;
            Assert.IsNotNull(resultado);
            Assert.AreEqual(expected: 404,actual: resultado.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Delete_Retorna204_CuandoAutorExiste(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = Substitute.For<IAlmacenadorArchivos>();
            ILogger<AutorServicio> logger = Substitute.For<ILogger<AutorServicio>>();
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            
            var autorServicio = ConstruirAutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);
            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            context.Add(new Autor { Nombres = "Ernest", Apellidos = "Hemingway", });

            await context.SaveChangesAsync();

            //Prueba
            var respuesta = await autoresController.Delete(autorId);

            //Validacion
            var resultado = respuesta as NoContentResult;

            Assert.IsNotNull(resultado);
            Assert.AreEqual(expected: 204, actual: resultado.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task Delete_DebeLlamarDeleteAutorAsyncDelServicioAutor(int autorId)
        {
            //Preparacion
            IOutputCacheStore outputCacheStore = Substitute.For<IOutputCacheStore>();
            IAutorServicio autorServicio = Substitute.For<IAutorServicio>();

            var autoresController = new AutoresController(autorServicio, outputCacheStore);

            //Prueba
            await autoresController.Delete(autorId);

            //Validacion
            await autorServicio.Received(1).DeleteAutorAsync(autorId);
        }
    }
}
