using System;
using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.Mappers;
using BibliotecaAPITests.Utilidades;

namespace BibliotecaAPITests.PruebasUnitarias.Servicios;

[TestClass]
public class LibroServicioPruebas : BasePruebas
{
    //Helpers de construccion
    protected RepositorioLibro ConstruirRepositorioLibro(ApplicationDbContext context)
    {
        return new RepositorioLibro(context);
    }

    protected LibroMapper ConstuirLibroMapper()
    {
        return new LibroMapper();
    }
}
