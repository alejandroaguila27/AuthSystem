using AuthSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure your connection string by reading it from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("AuthSystemDatabase");

// Add services to the container.
builder.Services.AddDbContext<AuthSystemDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("AuthSystem.Infrastructure"));
});

// Registrar tus servicios de aplicación y otros servicios necesarios.

// Asegúrate de registrar otros servicios e interfaces que necesites.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
