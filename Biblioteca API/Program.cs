using System.Text.Json.Serialization;
using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Area de servcios

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IRepositorioAutor, RepositorioAutor>();
builder.Services.AddScoped<IRepositorioLibro, RepositorioLibro>();
builder.Services.AddDbContext<ApplicationDbContext>(opciones => 
opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

//Area de middlewares

app.MapControllers();

app.Run();
