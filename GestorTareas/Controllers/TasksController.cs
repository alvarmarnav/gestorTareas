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
using GestorTareas.Helpers;


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
    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId:int}")] // GET /api/tareas
    public IActionResult GetAllTaskByUser(int userId)
    {
        return Ok(_taskManagerService.GetAllTasksByUser(userId));
    }
    /// <summary>
    /// Obtiene todas las tareas que pertenecen a un usuario mediante ID.
    /// </summary>
    /// <returns>Lista de tareas con el nombre del usuario asignado.</returns>
    [HttpGet("tasks")] // GET /api/tareas
    public IActionResult GetAllTaskOwnUser()
    {
        var claimUser = ClaimsPrincipal.Current;
        if(claimUser is null)
            return Unauthorized();
        var userId = UserConnectedHelper.GetConnectedUser(claimUser);
        
        return Ok(_taskManagerService.GetAllTasksByUser(userId));
    }
    /// <summary>
    /// Obtiene la tarea seleccioinada por ID.
    /// </summary>
    /// <returns>Tarea seleccionada con ID.</returns>
    [HttpGet("{taskId:int}")] // GET /api/tareas/1
    public IActionResult GetById(int taskId)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr is null) return Unauthorized();

        int.TryParse(userIdStr, out var userId);

        var task = _taskManagerService.GetTaskById(taskId);
        if (task == null) return NotFound();

        if (task.UserId != userId && !task.UsersList.Any(u => u.Id == userId) && !User.IsInRole("Admin"))
            return Unauthorized();

        return Ok(task);
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
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr is null) return NotFound();
        int userId = int.Parse(userIdStr);
        var result = _taskManagerService.GetPagination(pageNumber, itemsPerPage, userId);
        return Ok(result);
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
        //Diferenciar entre Propietario y Admin
        var userActiveStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userActiveStr is null) return Unauthorized();
        var userActiveId = int.Parse(userActiveStr);
        _taskManagerService.UpdateTask(id, taskDto, userActiveId);
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
    /// <summary>
    /// Añadir nuevo usuario a una CollaborativeTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPut("/collaborativeTask/{taskId:int}/{userId:int}")]
    public IActionResult AddNewTeamMember(int taskId, int userId)
    {
        _taskManagerService.AddNewTeamMember(taskId, userId);
        return NoContent();
    }
    /// <summary>
    /// Eliminar usuario de una CollaborativeTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPut("/collaborativeTaskDeleteUser/{taskId:int}/{userId:int}")]
    public IActionResult RemoveTeamMember(int taskId, int userId)
    {
        _taskManagerService.RemoveTeamMember(taskId, userId);
        return NoContent();
    }
}
