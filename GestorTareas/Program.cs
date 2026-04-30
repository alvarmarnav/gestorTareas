using System.Formats.Tar;
using System.Net;
using GestorTareas;
using GestorTareas.Application.Services;
using GestorTareas.Infraestructure.Repositories;
using CompositeTaskType = GestorTareas.Enums.CompositeTaskType;
using GestorTareas.Models;
using GestorTareas.Infraestructure.Data;
using Microsoft.VisualBasic;
using Task = GestorTareas.Models.Task;
using User = GestorTareas.Models.User;
using Microsoft.EntityFrameworkCore;
using var context = new GestorTareasContext();


var usuario = new User(userName:"Juanito",userLastName:"Mueller",userEmail:"juanin@hotmail.com",isActive:true,isAdmin:false)
{};

var tarea = new SimpleTask(title:"Tarea Simple 1", "Descripcion")
{
    UserId = usuario.Id,
};

User? userToFind = context.Users.Find(usuario.Id);

List<Task> tareasPendientes = context.Tasks
.Where(t => t.Status != GestorTareas.Enums.TaskStatus.Completed)
.OrderBy(t => t.CreatedAt)
.ToList();

List<User> conTareas = context.Users
.Include(u => u.tasksList)
.ToList();

context.SaveChanges();

Console.WriteLine($"Usuario creado con Id: {usuario.Id}");

Console.WriteLine($"Tareas creada con exito: {tarea.Title}");

if(tareasPendientes.Any())
    Console.WriteLine($"exito: {tareasPendientes}");
else
    Console.WriteLine("nada de nada");
// Console.WriteLine(tareasPendientes.FirstOrDefault());
// Console.WriteLine(conTareas.First());

foreach (var i in tareasPendientes)
{
    Console.Write(3);
    // i.ResumeTask();
}


foreach (var item in conTareas)
{
    item.ToString();
}

// Console.WriteLine(tarea.ResumeTask());

