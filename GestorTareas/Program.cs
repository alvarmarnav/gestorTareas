using System.Formats.Tar;
using System.Net;
using GestorTareas;
using GestorTareas.Application.Services;
using GestorTareas.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

// PARTE 1: registrar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Antes de builder.Build()
builder.Services.AddDbContext<GestorTareasContext>
(options =>
options.UseSqlServer(
builder.Configuration
.GetConnectionString("GestorTareas")
)
);

var app = builder.Build();

// PARTE 2: configurar el pipeline de peticiones
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); // arranca el servidor y se queda