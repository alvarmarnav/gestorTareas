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
    [Authorize]
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
    /// Obtiene DTO todas las tareas.
    /// </summary>
    /// <returns>Obtiene el DTO de todas las tarea..</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponseDto<ResponseTaskDto>), 200)]
    public IActionResult GetAll(
        [FromQuery] int actualPage = 1,
        [FromQuery] int itemsPerPage = 10)
    {
        var result = _taskManagerService.GetPagination(actualPage, itemsPerPage, UserConnectedHelper.GetConnectedUser(User));
        return Ok(result);
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
    /// Crea una nueva tarea
    /// </summary>
    [HttpPost("simple")] // POST /api/tareas
    [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Create([FromBody] CreateSimpleTaskDto dto)
    {
        var task = _taskManagerService.CreateTask(dto, UserConnectedHelper.GetConnectedUser(User));
        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, task);
    }
    /// <summary>
    /// Crear nueva tarea Recurrente
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("recurring")]
    public ActionResult<List<ResponseRecurringTaskDto>> CreateRecurringTask([FromBody] CreateRecurringTaskDto dto)
    {
        var response = _taskManagerService.CreateRecurringTask(dto, UserConnectedHelper.GetConnectedUser(User));
        return Created(string.Empty, response);
    }
    /// <summary>
    /// Crear nueva tarea Composite
    /// </summary>
    /// <param name="compDto"></param>
    /// <returns></returns>
    [HttpPost("composite")]
    [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CreateCompositeTask([FromBody] CreateCompositeTaskDto compDto)
    {
        var task = _taskManagerService.CreateCompositeTask(compDto, UserConnectedHelper.GetConnectedUser(User));

        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, task);
    }
    /// <summary>
    /// Crear CollaborativeTask
    /// </summary>
    /// <param name="collDto"></param>
    /// <returns></returns>
    [HttpPost("collaborative")]
    [ProducesResponseType(typeof(TaskDTO),
    StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CreateCollaborativeTask([FromBody] CreateCollaborativeTaskDto collDto)
    {
        var task = _taskManagerService.CreateCollaborativeTask(collDto, UserConnectedHelper.GetConnectedUser(User));
        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, task);
    }
    /// <summary>
    /// Añadir una subtask
    /// </summary>
    /// <param name="compositeTaskId"></param>
    /// <param name="subtaskDto"></param>
    /// <returns></returns>
    [HttpPost("{compositeTaskId:int}/subtasks")]
    [ProducesResponseType(typeof(TaskDTO),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddSubtask(int compositeTaskId, [FromBody] CreateSubTaskDto subtaskDto)
    {
        var task = _taskManagerService.CreateSubTask(compositeTaskId, subtaskDto, UserConnectedHelper.GetConnectedUser(User));
        return CreatedAtAction(nameof(GetById), new { taskId = task.Id }, task);
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
    /// Añadir nuevo usuario a una CollaborativeTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="taskCollaboratorDto"></param>
    /// <returns></returns>
    [HttpPost("{taskId:int}/collaborators")]
    public IActionResult AddTaskCollaborator(int taskId, [FromBody] CreateTaskCollaboratorDto taskCollaboratorDto)
    {
        _taskManagerService.AddTaskCollaborator(taskId, taskCollaboratorDto, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    /// <summary>
    /// Actualiza la Tarea seleccionada por Id
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="taskDto"></param>
    /// <returns></returns>
    [HttpPut("{taskId:int}")] // PUT /api/tareas/1
    public IActionResult Update(int taskId, [FromBody] UpdateTaskDto taskDto)
    {
        _taskManagerService.UpdateTask(taskId, taskDto, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    /// <summary>
    /// Elimina una tarea seleccionada por ID
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpDelete("{taskId:int}")] // DELETE /api/tareas/1
    // [Authorize(Roles = "Admin")]
    public IActionResult Delete(int taskId)
    {
        _taskManagerService.DeleteTask(taskId, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    /// <summary>
    /// Ruta para eliminar relaciones de linkedTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="linkedTaskId"></param>
    /// <returns></returns>
    [HttpDelete("{taskId:int}/linkedRelation/{linkedTaskId:int}")]
    public IActionResult DeleteLinkedRelation(int taskId, int linkedTaskId)
    {
        _taskManagerService.DeleteLinkedRelation(taskId, linkedTaskId, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    /// <summary>
    /// Eliminar usuario de una CollaborativeTask
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{taskId:int}/collaborators/{userId:int}")]
    public IActionResult RemoveTaskCollaborator(int taskId, int userId)
    {
        _taskManagerService.RemoveTaskCollaborator(taskId, userId, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
    /// <summary>
    /// Marca una tarea seleccionada como completada por su ID
    /// </summary>
    [HttpPut("{taskId:int}/complete")] // PUT /api/tasks/1/complete
    public IActionResult CompleteTask(int taskId)
    {
        // Asegúrate de tener este método implementado en tu servicio de aplicación
        _taskManagerService.CompleteTask(taskId, UserConnectedHelper.GetConnectedUser(User));
        return NoContent();
    }
[HttpGet("{taskId:int}/linkable")]
public IActionResult GetLinkableTasksById(int taskId)
    {
        return  Ok(_taskManagerService.GetLinkableTaskById(taskId, UserConnectedHelper.GetConnectedUser(User)));
    }
}
