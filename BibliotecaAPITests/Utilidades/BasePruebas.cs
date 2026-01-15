using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca_API.Datos;
using Biblioteca_API.Mappers;
using Microsoft.EntityFrameworkCore;

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

        protected AutorMapper ConstruirMapper()
        {
            var autorMapper = new AutorMapper();
            return autorMapper;
        }
    }
}
