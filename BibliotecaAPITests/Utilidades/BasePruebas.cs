using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca_API.Datos;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPITests.Utilidades
{
    public class BasePruebas
    {
        protected ApplicationDbContext ConstruirContext(string nombreBD)
        {
            var opcioones = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(nombreBD).Options;

            var dbContext = new ApplicationDbContext(opcioones);
            return dbContext;
        }
    }
}
