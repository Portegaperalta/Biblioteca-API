using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.Mappers;
using Biblioteca_API.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BibliotecaAPITests.Utilidades
{
    public class BasePruebas
    {
        protected ApplicationDbContext ConstruirContext(string nombreBD)
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(nombreBD).Options;

            var dbContext = new ApplicationDbContext(opciones);
            return dbContext;
        }

        protected RepositorioAutor ConstruirRepositorioAutor(ApplicationDbContext context)
        {
            var repositorioAutor = new RepositorioAutor(context);
            return repositorioAutor;
        }

        protected AutorServicio ConstruirAutorServicio(IRepositorioAutor repositorioAutor, AutorMapper autorMapper,
                             IAlmacenadorArchivos almacenadorArchivos, ILogger<AutorServicio> logger)
        {
            var autorServicio = new AutorServicio(repositorioAutor, autorMapper, almacenadorArchivos, logger);
            return autorServicio;
        }

        protected AutorMapper ConstruirMapper()
        {
            var autorMapper = new AutorMapper();
            return autorMapper;
        }
    }
}
