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
        return Ok(_taskManagerService.GetAllTasksByUser(userId, UserConnectedHelper.GetConnectedUser(User)));
    }
    /// <summary>
    /// Obtiene todas las tareas que pertenecen a un usuario mediante ID.
    /// </summary>
    /// <returns>Lista de tareas con el nombre del usuario asignado.</returns>
    [HttpGet("tasks")] // GET /api/tareas
    public IActionResult GetAllTaskOwnUser()
    {
        return Ok(_taskManagerService.GetAllTaskOwnUser(UserConnectedHelper.GetConnectedUser(User)));
    }
    /// <summary>
    /// Obtiene la tarea seleccioinada por ID.
    /// </summary>
    /// <returns>Tarea seleccionada con ID.</returns>
    [HttpGet("{taskId:int}")] // GET /api/tareas/1
    public IActionResult GetById(int taskId)
    {
        var task = _taskManagerService.GetTaskById(taskId, UserConnectedHelper.GetConnectedUser(User));
        if (task == null) return NotFound();

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
        var result = _taskManagerService.GetPagination(pageNumber, itemsPerPage, UserConnectedHelper.GetConnectedUser(User));
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
    public IActionResult Create([FromBody] CreateSimpleTaskDto dto)
    {
        var task = _taskManagerService.CreateTask(dto, UserConnectedHelper.GetConnectedUser(User));

        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, dto);
        // return CreatedAtAction(nameof(GetById), new { id = task.Id }, tarea);
    }

/// <summary>
/// Crear Nueva Dependencia de LinkedTask
/// </summary>
/// <param name="taskId"></param>
/// <param name="dto"></param>
/// <returns></returns>
    [Authorize]
    [HttpPost("{taskId:int}/linkedRelation")]
    public IActionResult AddLinkedTaskRelation(int taskId, [FromBody] CreateLinkedTaskRelationDto dto)
    {
        var linkedTaskRelation = _taskManagerService.AddLinkedTask(taskId, dto.DependsOnTaskId, dto.LinkedTaskOrder, UserConnectedHelper.GetConnectedUser(User));
        return Ok(linkedTaskRelation);
    }
    
    /// <summary>
    /// Crear CollaborativeTask
    /// </summary>
    /// <param name="collDto"></param>
    /// <returns></returns>
    [HttpPost("tasks/collaborative")]
    [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CreateCollaborativeTask([FromBody] CreateCollaborativeTaskDto collDto)
    {
        var task = _taskManagerService.CreateCollaborativeTask(collDto,UserConnectedHelper.GetConnectedUser(User) );

        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, collDto);
    }
/// <summary>
    /// Añadir nuevo usuario a una CollaborativeTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="taskCollaboratorDto"></param>
    /// <returns></returns>
    [HttpPut("/collaborativeTask/{taskId:int}/collaborators")]
    public IActionResult AddTaskCollaborator(int taskId, [FromBody] CreateTaskCollaboratorDto taskCollaboratorDto)
    {
        _taskManagerService.AddTaskCollaborator(taskId, taskCollaboratorDto, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }

/// <summary>
    /// Crear CompositeTask
    /// </summary>
    /// <returns></returns>
    [HttpPost("tasks/composite")]
    [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CreateCompositeTask([FromBody] CreateCompositeTaskDto compDto)
    {
        var task = _taskManagerService.CreateCompositeTask(compDto,UserConnectedHelper.GetConnectedUser(User) );

        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, compDto);
    }


[HttpPost("/compositeTasks/{compositeTaskId:int}/subtasks")]
 [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddSubtask(int compositeTaskId,[FromBody] CreateSubTaskDto subtaskDto)
    {
         var task = _taskManagerService.CreateSubTask(compositeTaskId,subtaskDto,UserConnectedHelper.GetConnectedUser(User) );

        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, subtaskDto);
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
        _taskManagerService.UpdateTask(id, taskDto, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    /// <summary>
    /// Elimina una tarea seleccionada por ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")] // DELETE /api/tareas/1
    // [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _taskManagerService.DeleteTask(id, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    
    /// <summary>
    /// Eliminar usuario de una CollaborativeTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="taskCollaboratorDto"></param>
    /// <returns></returns>
    [HttpPut("/collaborativeTaskDeleteUser/{taskId:int}")]
    public IActionResult RemoveTaskCollaborator(int taskId, [FromBody] RemoveTaskCollaboratorDto taskCollaboratorDto)
    {
        _taskManagerService.RemoveTaskCollaborator(taskId, taskCollaboratorDto, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }



}
