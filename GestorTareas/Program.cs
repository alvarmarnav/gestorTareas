
using GestorTareas;
using GestorTareas.Application.Services;
using GestorTareas.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using GestorTareas.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// PARTE 1: registrar servicios
builder.Services.AddControllers();

// Antes de builder.Build()
builder.Services.AddDbContext<GestorTareasContext>
(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("GestorTareas")
)
);

builder.Services.AddScoped<ITaskRepository, TaskRepositoryEF>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserManagerService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Emisor"],
        ValidAudience = builder.Configuration["Jwt:Audiencia"],
        IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ClaveSecreta"]!))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// PARTE 2: configurar el pipeline de peticiones
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestorTareas API v1");
        c.RoutePrefix = "";
    });
}
app.UseHttpsRedirection();
app.UseAuthentication(); // primero identifica al usuario
app.UseAuthorization(); // luego comprueba sus permisos

app.MapControllers();

app.Run(); // arranca el servidor y se queda