using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs.Autor;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Biblioteca_API.Servicios;
using BibliotecaAPITests.Utilidades;
using Microsoft.Extensions.Logging;

namespace BibliotecaAPITests.PruebasUnitarias.Servicios
{
    [TestClass]
    public class AutorServicioPruebas : BasePruebas
    {
        // helpers de construccion
        protected RepositorioAutor ConstruirRepositorioAutor(ApplicationDbContext context)
        {
            return new RepositorioAutor(context);
        }

        protected AutorServicio ConstruirAutorServicio(IRepositorioAutor repositorioAutor, AutorMapper autorMapper,
                             IAlmacenadorArchivos almacenadorArchivos, ILogger<AutorServicio> logger)
        {
            return new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);
        }

        protected AutorMapper ConstruirMapper()
        {
            return new AutorMapper();
        }

        [TestMethod]
        [DataRow(1)]
        public async Task GetAutorDtoAsync_RetornarNull_CuandoAutorDesdeBDEsNull(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper,almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.GetAutorDtoAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: null, actual: resultado);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task GetAutorDtoAsync_RetornarAutorDTO_CuandoAutorDesdeDBNoEsNull(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            context.Add(new Autor{Nombres = "Ernest",Apellidos = "Hemingway",});
            context.Add(new Autor { Nombres = "Pablo", Apellidos = "Neruda", });

            await context.SaveChangesAsync();

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.GetAutorDtoAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: 1, actual: resultado!.Id);
        }
    }
}
