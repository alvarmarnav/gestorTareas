
using GestorTareas;
using GestorTareas.Application.Services;
using GestorTareas.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using GestorTareas.Interfaces;



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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    
});

// builder.Services.AddGrpc().AddJsonTranscoding();
// builder.Services.AddGrpcSwagger();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1",
//         new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
// });

// builder.Services.AddSwagger();

// // builder.Services.AddSwaggerGen(options =>
// // {
// // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
// // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
// // options.IncludeXmlComments(xmlPath);
// // });
// try
// {
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
app.UseAuthorization();

app.MapControllers();

app.Run(); // arranca el servidor y se queda
// }
// catch (ReflectionTypeLoadException ex)
// {
//      foreach (var e in ex.LoaderExceptions)
//     {
//         Console.WriteLine("ERROR REAL: " + e.Message);
//     }
// }