
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
using System.Reflection;
using GestorTareas.Infraestructure.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// PARTE 1: registrar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly
    .GetExecutingAssembly()
    .GetName().Name}.xml";
    var xmlPath = Path.Combine(
    AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
{
new OpenApiSecurityScheme
{
Reference = new OpenApiReference
{
Type = ReferenceType.SecurityScheme,
Id = "Bearer"
}
},
Array.Empty<string>()
}
});
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Antes de builder.Build() EF CORE
builder.Services.AddDbContext<GestorTareasContext>
(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("GestorTareas")
)
);
builder.Services.AddTransient<UsersSeeder>();
builder.Services.AddTransient<TaskSeeder>();

//REPOSITORIOS CON SUS INTERFACES
builder.Services.AddScoped<ITaskRepository, TaskRepositoryEF>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//SERVICES
builder.Services.AddScoped<UserManagerService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskManagerService>();
//JWT Autenticación
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

// MIGRATIONS + SEEDERS
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // var context = services.GetRequiredService<GestorTareasContext>();
    // // Crear BD y ejecutar migrations pendientes
    // await context.Database.MigrateAsync();

    var seeder = services.GetRequiredService<UsersSeeder>();
    await seeder.AsyncSeeder();
    var seeder2 = services.GetRequiredService<TaskSeeder>();
    await seeder2.AsyncSeeder();
}

app.Run(); // arranca el servidor y se queda