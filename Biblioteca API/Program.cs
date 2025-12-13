using System.Text.Json.Serialization;
using Biblioteca_API.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Area de servcios

builder.Services.AddControllers().AddJsonOptions(opciones => 
        opciones.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ApplicationDbContext>(opciones => 
        opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

//Area de middlewares

app.MapControllers();

app.Run();
