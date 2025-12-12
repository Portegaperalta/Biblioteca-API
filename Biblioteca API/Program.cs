var builder = WebApplication.CreateBuilder(args);

// Area de servcios

builder.Services.AddControllers();

var app = builder.Build();

//Area de middlewares

app.MapControllers();

app.Run();
