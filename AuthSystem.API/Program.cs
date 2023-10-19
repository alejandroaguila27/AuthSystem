using AuthSystem.Application.Services;
using AuthSystem.Domain.Interfaces;
using AuthSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexi�n ley�ndola desde appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("AuthSystemDatabase");

// A�adir servicios al contenedor de servicios.
builder.Services.AddDbContext<AuthSystemDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("AuthSystem.Infrastructure"));
});

// Registrar tus servicios de aplicaci�n y otras dependencias.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();

// Obtener el ambiente de desarrollo (Development Environment)
var environment = builder.Environment;

// Agregar soporte para Swagger/OpenAPI solo en entorno de desarrollo
if (environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// Configurar la tuber�a de solicitud (request pipeline) HTTP.
if (environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
