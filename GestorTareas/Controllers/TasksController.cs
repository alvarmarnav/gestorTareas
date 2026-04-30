using System;
using GestorTareas.Infraestructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Controllers;


[ApiController]
[Route("api/[Controller]")]
public class TasksController : ControllerBase
{

    [HttpGet] // GET /api/tareas
    public IActionResult GetAll() { 
        

        return Ok(tasks);
     }

    [HttpGet("{id}")] // GET /api/tareas/1
    public IActionResult GetById(int id) { ... }

    [HttpPost] // POST /api/tareas
    public IActionResult Create(...) { ... }

    [HttpPut("{id}")] // PUT /api/tareas/1
    public IActionResult Update(int id, ...) { ... }

    [HttpDelete("{id}")] // DELETE /api/tareas/1
    public IActionResult Delete(int id) { ... }
}
