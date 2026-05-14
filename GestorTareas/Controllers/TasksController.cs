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

    // [HttpGet] // GET /api/tareas
    // public IActionResult GetAll()
    // {
    //     // var claimUser = System.Security.Claims.ClaimsPrincipal.Current;
    //     var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (userIdStr is null) return NotFound();

    //     return Ok(_taskManagerService.GetAllTasks());
    // }
    /// <summary>
    /// Obtiene todas las tareas que pertenecen a un usuario mediante ID.
    /// </summary>
    /// <returns>Lista de tareas con el nombre del usuario asignado.</returns>
    [HttpGet("user/{userId:int}")] // GET /api/tareas
    public IActionResult GetAllTaskByUser()
    {
        // var claimUser = System.Security.Claims.ClaimsPrincipal.Current;
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr is null) return NotFound();

        int userId = int.Parse(userIdStr);

        return Ok(_taskManagerService.GetAllTasksByUser(userId));
    }
    /// <summary>
    /// Obtiene la tarea seleccioinada por ID.
    /// </summary>
    /// <returns>Tarea seleccionada con ID.</returns>
    [HttpGet("taskId/{taskId:int}")] // GET /api/tareas/1
    public IActionResult GetById(int id)
    {
        var task = _taskManagerService.GetTaskById(id);
        if (task == null) return NotFound();
        return Ok(_taskManagerService.GetTaskById(id));
    }
    /// <summary>
    /// Obtiene DTO todas las tareas.
    /// </summary>
    /// <returns>Obtiene el DTO de todas las tarea..</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponseDto<ResponseTaskDto>), 200)]
    public IActionResult GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int itemsPerPage = 10)
    {
        var resultado = _taskManagerService.GetPagination(pageNumber, itemsPerPage);
        return Ok(resultado);
    }
    /// <summary>
    /// Crea una nueva tarea
    /// </summary>
    [HttpPost] // POST /api/tareas
    [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        dto.LinkedTaskOrder,
        dto.RecurrenceRule,
        dto.TaskSupervisor
        );

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        // return CreatedAtAction(nameof(GetById), new { id = task.Id }, tarea);
    }
    /// <summary>
    /// Actualiza la Tarea seleccionada por Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="taskDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")] // PUT /api/tareas/1
    public IActionResult Update(int id, [FromBody] UpdateTaskDto taskDto)
    {
        _taskManagerService.UpdateTask(id, taskDto);

        return NoContent();
    }
    /// <summary>
    /// Elimina una tarea seleccionada por ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")] // DELETE /api/tareas/1
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _taskManagerService.DeleteTask(id);
        return NoContent();
    }
}
