using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = GestorTareas.Models.Task;


namespace GestorTareas.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]//Aplica a todos endpoints clase, luego override especificamente
public class TasksController : ControllerBase
{

    private readonly TaskManagerService _taskManagerService;

    public TasksController(TaskManagerService taskManagerService) => _taskManagerService = taskManagerService;

    [HttpGet] // GET /api/tareas
    public IActionResult GetAll()
    {
        // var claimUser = System.Security.Claims.ClaimsPrincipal.Current;
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userIdStr is null) return NotFound();

        int userId = int.Parse(userIdStr);

        return Ok(_taskManagerService.GetAllTasks(userId));
    }

    [HttpGet("{id}")] // GET /api/tareas/1
    public IActionResult GetById(int id)
    {
        var task = _taskManagerService.GetTaskById(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost] // POST /api/tareas
    public IActionResult Create([FromBody] CreateTaskDto dto)
    {
        //Obtener ID del usuario
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr is null) return Unauthorized();

        int userId = int.Parse(userIdStr);

        var task = _taskManagerService.AddTask(
        dto.Title,
        userId,
        dto.TaskDescription,
        dto.Priority,
        dto.Status,
        dto.DueTime,
        dto.CancelReason,
        dto.CompositeTaskType,
        dto.RecurrenceRule,
        dto.LinkedTaskOrder,
        dto.TaskSupervisor
        );

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        // return CreatedAtAction(nameof(GetById), new { id = task.Id }, tarea);
    }

    [HttpPut("{id}")] // PUT /api/tareas/1
    public IActionResult Update(int id, [FromBody] UpdateTaskDto taskDto)
    {
        _taskManagerService.UpdateTask(id, taskDto);

        return NoContent();
    }

    [HttpDelete("{id}")] // DELETE /api/tareas/1
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _taskManagerService.DeleteTask(id);
        return NoContent();
    }
}
