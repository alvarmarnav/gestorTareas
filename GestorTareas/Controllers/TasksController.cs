using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
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
    public IActionResult GetAll()
    {
        return Ok(_taskManagerService.GetAllTasks());
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
        var task = _taskManagerService.AddTask(
        dto.Title,
        dto.UserId,
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
        _taskManagerService.UpdateTask(id,taskDto);

        return NoContent();
    }

    [HttpDelete("{id}")] // DELETE /api/tareas/1
    public IActionResult Delete(int id)
    {
        _taskManagerService.DeleteTask(id);
        return NoContent();
    }
}
