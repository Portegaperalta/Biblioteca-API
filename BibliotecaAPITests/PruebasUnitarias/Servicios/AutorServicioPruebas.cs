using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.DTOs;
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

        protected AutorMapper ConstruirMapper()
        {
            return new AutorMapper();
        }

        [TestMethod]
        public async Task  GetAutoresDtoAsync_RetornaIenumerableAutorDTO()
        {
             //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            context.Add(new Autor { Nombres = "Ernest", Apellidos = "Hemingway", });
            context.Add(new Autor { Nombres = "Pablo", Apellidos = "Neruda", });

            await context.SaveChangesAsync();

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);
            var autor1 = await autorServicio.GetAutorDtoAsync(1);
            var autor2 = await autorServicio.GetAutorDtoAsync(2);
            var lista = new List<AutorDTO>();

            lista.Add(autor1!);
            lista.Add(autor2!);

            var paginacionDTO = new PaginacionDTO();

            //Prueba
            var resultado = await autorServicio.GetAutoresDtoAsync(paginacionDTO);

            //Validacion
            Assert.IsInstanceOfType<IEnumerable<AutorDTO>>(resultado);
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
        public async Task GetAutorAsync_RetornarNull_CuandoAutorIdNoExiste(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.GetAutorAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: null, actual: resultado);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task GetAutorAsync_RetornarAutor_CuandoAutorIdExiste(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            context.Add(new Autor { Nombres = "Ernest", Apellidos = "Hemingway", });
            context.Add(new Autor { Nombres = "Pablo", Apellidos = "Neruda", });

            await context.SaveChangesAsync();

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.GetAutorAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: 1, actual: resultado!.Id);
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

        [TestMethod]
        [DataRow(1)]
        public async Task GetAutorSinLibrosDtoAsync_RetornaNull_CuandoAutorIdNoExiste(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.GetAutorSinLibrosDtoAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: null, actual: resultado);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task GetAutorSinLibrosDtoAsync_RetornaAutorSinLibroDTO_CuandoAutorIdExiste(int autorId)
        {
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            context.Add(new Autor { Nombres = "Ernest", Apellidos = "Hemingway", });
            context.Add(new Autor { Nombres = "Pablo", Apellidos = "Neruda", });

            await context.SaveChangesAsync();

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.GetAutorSinLibrosDtoAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: 1, actual: resultado!.Id);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task DeleteAutorAsync_RetornaFalso_CuandoAutorNoExiste(int autorId)
        {
            
            //Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreDB);
            var repositorioAutor = ConstruirRepositorioAutor(context);
            var autorMapper = ConstruirMapper();
            IAlmacenadorArchivos almacenadorArchivos = null!;
            ILogger<AutorServicio> logger = null!;

            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);

            //Prueba
            var resultado = await autorServicio.DeleteAutorAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: false, actual: resultado);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task DeleteAutorAsync_RetornaTrue_CuandoAutorExisteYEsEliminado(int autorId)
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
            var resultado = await autorServicio.DeleteAutorAsync(autorId);

            //Validacion
            Assert.AreEqual(expected: true, actual: resultado);
        }
    }
}
