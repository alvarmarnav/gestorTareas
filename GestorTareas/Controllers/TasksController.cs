using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Infraestructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = GestorTareas.Models.Task;


namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class TasksController : ControllerBase
{

private readonly TaskManagerService _taskManagerService;

public TasksController(TaskManagerService taskManagerService) => _taskManagerService = taskManagerService;

    [HttpGet] // GET /api/tareas
    public IActionResult GetAll() { 
        return Ok(_taskManagerService.GetAllTasks());
     }

    [HttpGet("{id}")] // GET /api/tareas/1
    public IActionResult GetById(Guid id)
    {
        var task = _taskManagerService.GetTaskById(id);
        if(task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost] // POST /api/tareas
    public IActionResult Create([FromBody] CreateTaskDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")] // PUT /api/tareas/1
    public IActionResult Update(Guid id) { throw new NotImplementedException(); }

    [HttpDelete("{id}")] // DELETE /api/tareas/1
    public IActionResult Delete(Guid id) {  throw new NotImplementedException(); }
}
