using System.Text;
using Biblioteca_API.Datos;
using Biblioteca_API.Datos.Repositorios;
using Biblioteca_API.Entidades;
using Biblioteca_API.Mappers;
using Biblioteca_API.Servicios;
using Biblioteca_API.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Area de servcios

//builder.Services.AddOutputCache(opciones =>
//{
//    opciones.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(15);
//});

builder.Services.AddStackExchangeRedisOutputCache(opciones =>
{
    opciones.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddDataProtection();

var origenesPermitidos = builder.Configuration
                                .GetSection("origenesPermitidos")
                                .Get<string[]>()!;

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(origenesPermitidos)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("cantidad-total-registros");
    });
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<LibroMapper>();
builder.Services.AddScoped<AutorMapper>();
builder.Services.AddScoped<UsuarioMapper>();
builder.Services.AddTransient<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddScoped<IAutorServicio, AutorServicio>();
builder.Services.AddScoped<ILibroServicio, LibroServicio>();
builder.Services.AddScoped<IComentarioServicio, ComentarioServicio>();
builder.Services.AddScoped<IRepositorioAutor, RepositorioAutor>();
builder.Services.AddScoped<IRepositorioLibro, RepositorioLibro>();
builder.Services.AddScoped<IRepositorioComentario, RepositorioComentario>();

builder.Services.AddDbContext<ApplicationDbContext>(opciones => 
opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentityCore<Usuario>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<Usuario>>();
builder.Services.AddScoped<SignInManager<Usuario>>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;

    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llaveJwt"]!)),
        ClockSkew = TimeSpan.Zero
    };

});

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esAdmin", politica => politica.RequireClaim("esAdmin"));
}
);

builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Description = "WebAPI para trabajar con datos de autores y libros",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        { 
         Email = "ortegaperaltap@gmail.com",
         Name = "Pablo Ortega",
         Url = new Uri("https://github.com/Portegaperalta")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        { 
          Name = "MIT",
          Url = new Uri("https://opensource.org/license/mit/")
        }
    });

    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    opciones.OperationFilter<FiltroAutorizacion>();
});

var app = builder.Build();

//Area de middlewares

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.MapControllers();

app.Run();
