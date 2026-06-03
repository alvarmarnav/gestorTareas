using GestorTareas.Application.DTOs;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Moq;
using NUnit.Framework;
using Task = GestorTareas.Models.Task;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Tests;

[TestFixture]
public class TaskManagerServiceTests
{
    private Mock<ITaskRepository> _mockRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private TaskManagerService _taskService;
    private readonly CurrentUserDto _taskOwnerA = new CurrentUserDto
    {
        CurrentUserId = 2,
        CurrentUserSystemRole = Enums.SystemRole.User,
        CurrentUserTaskRole = Enums.CollaboratorRole.Viewer,
    };
    private readonly CurrentUserDto _taskOwnerB = new CurrentUserDto
    {
        CurrentUserId = 3,
        CurrentUserSystemRole = Enums.SystemRole.User,
        CurrentUserTaskRole = Enums.CollaboratorRole.Viewer,
    };
    private readonly CurrentUserDto _taskOwnerC = new CurrentUserDto
    {
        CurrentUserId = 4,
        CurrentUserSystemRole = Enums.SystemRole.User,
        CurrentUserTaskRole = Enums.CollaboratorRole.Viewer,
    };
    private readonly CurrentUserDto _adminUser = new CurrentUserDto
    {
        CurrentUserId = 100,
        CurrentUserSystemRole = Enums.SystemRole.Admin,
        CurrentUserTaskRole = Enums.CollaboratorRole.TaskAdministrator,
    };

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _mockUserRepository = new Mock<IUserRepository>();

        _taskService = new TaskManagerService(_mockRepository.Object, _mockUserRepository.Object);

        _mockUserRepository.Setup(r => r.GetUserById(_taskOwnerA.CurrentUserId))
        .Returns(new User
        {
            Id = _taskOwnerA.CurrentUserId,
            UserName = "Usuario Prueba A",
            UserLastName = "Lastname Pruebas A",
            UserEmail = "usuarioA@email.com",
            IsActive = true,
            IsAdmin = false
        });
        _mockUserRepository.Setup(r => r.GetUserById(_taskOwnerB.CurrentUserId))
       .Returns(new User
       {
           Id = _taskOwnerB.CurrentUserId,
           UserName = "Usuario Prueba B",
           UserLastName = "Lastname Pruebas B",
           UserEmail = "usuarioB@email.es",
           IsActive = true,
           IsAdmin = false
       });
        _mockUserRepository.Setup(r => r.GetUserById(_taskOwnerC.CurrentUserId))
       .Returns(new User
       {
           Id = _taskOwnerC.CurrentUserId,
           UserName = "Usuario Prueba C",
           UserLastName = "Lastname Pruebas C",
           UserEmail = "usuarioC@email.com",
           IsActive = true,
           IsAdmin = false
       });
        _mockUserRepository.Setup(r => r.GetUserById(_adminUser.CurrentUserId))
       .Returns(new User
       {
           Id = _adminUser.CurrentUserId,
           UserName = "ADMIN de Prueba ",
           UserLastName = "Lastname ADMIN Pruebas",
           UserEmail = "administrator@email.com",
           IsActive = true,
           IsAdmin = true
       });

    }
    private static CurrentUserDto UserDto(int userId) => new()
    {
        CurrentUserId = userId,
        CurrentUserSystemRole = SystemRole.User,
        CurrentUserTaskRole = CollaboratorRole.Viewer
    };

    private static CurrentUserDto AdminDto(int userId) => new()
    {
        CurrentUserId = userId,
        CurrentUserSystemRole = SystemRole.Admin,
        CurrentUserTaskRole = CollaboratorRole.TaskAdministrator
    };

    private static User ActiveUser(int id, bool isAdmin = false) => new()
    {
        Id = id,
        UserName = $"User{id}",
        UserLastName = "Prueba",
        UserEmail = $"user{id}@test.com",
        IsActive = true,
        IsAdmin = isAdmin
    };

    private static SimpleTask Simple(int id, int ownerId, Enums.TaskStatus taskStatus = Enums.TaskStatus.Pending) => new()
    {
        Id = id,
        UserId = ownerId,
        Title = $"Simple {id}",
        TaskDescription = "Descripción de prueba",
        TaskType = TaskType.SimpleTask,
        TaskPriority = Priority.Normal,
        TaskStatus = taskStatus,
        DueTime = DateTime.UtcNow.AddDays(10)
    };

    private static CompositeTask Composite(int id, int ownerId, params SubTask[] subTasks) => new()
    {
        Id = id,
        UserId = ownerId,
        Title = $"Compuesta {id}",
        TaskDescription = "Tarea compuesta de prueba",
        TaskType = TaskType.CompositeTask,
        TaskPriority = Priority.Normal,
        TaskStatus = Enums.TaskStatus.Pending,
        DueTime = DateTime.UtcNow.AddDays(10),
        SubTaskList = subTasks.ToList()
    };

    private static SubTask SubTask(int id, int ownerId, int parentId, TaskStatus taskStatus) => new()
    {
        Id = id,
        UserId = ownerId,
        Title = $"Subtarea {id}",
        TaskDescription = "Subtarea de prueba",
        TaskType = TaskType.SubTask,
        TaskPriority = Priority.Normal,
        TaskStatus = taskStatus,
        DueTime = DateTime.UtcNow.AddDays(5),
        ParentCompositeTaskId = parentId
    };

    private static CollaborativeTask Collaborative(int id, int ownerId, params TaskCollaborator[] collaborators)
    {
        var newTask = new CollaborativeTask
        {
            Id = id,
            UserId = ownerId,
            Title = $"Colaborativa {id}",
            TaskDescription = "Tarea colaborativa de prueba",
            TaskType = TaskType.CollaborativeTask,
            TaskPriority = Priority.Normal,
            TaskStatus = TaskStatus.Pending,
            DueTime = DateTime.UtcNow.AddDays(10),
            TaskCollaborators = collaborators.ToList()
        };

        foreach (var collaborator in newTask.TaskCollaborators)
        {
            collaborator.TaskId = newTask.Id;
            collaborator.Task = newTask;
        }

        return newTask;
    }

    private static TaskCollaborator Collaborator(int userId, CollaboratorRole role) => new()
    {
        UserId = userId,
        CollaboratorRole = role,
        AddedAt = DateTime.UtcNow,
        UserTask = ActiveUser(userId)
    };

    private static CreateSubTaskDto CreateSubTaskDto(string title = "Subtarea") => new()
    {
        Title = title,
        TaskDescription = "Descripción subtarea",
        TaskPriority = Priority.Normal,
        DueTime = DateTime.UtcNow.AddDays(3)
    };

    private static UpdateTaskDto UpdateDto(
        string? title = null,
        string? description = null,
        Priority? taskPriority = null,
        DateTime? dueTime = null,
        int? recurrenceRule = null) => new(
            title,
            description,
            taskPriority,
            dueTime,
            linkedTaskOrder: null,
            recurrenceRule: recurrenceRule,
            cancelReason: null);



    [Test]
    public void CreateSimpleTask_PassTheCorrectValueToRepo_AssignCurrentUser()
    {
        var taskDto = new CreateSimpleTaskDto
        {
            Title = "Tarea SImple 1",
            TaskDescription = "Descripción pruebas tarea simple 1",
            TaskPriority = Enums.Priority.Low,
            DueTime = DateTime.UtcNow.AddDays(15)
        };

        _mockRepository
            .Setup(r => r.CreateTask(It.IsAny<Task>()))
            .Returns((Task task) =>
            {
                task.Id = 100;
                return task;
            });

        var result = _taskService.CreateTask(taskDto, _taskOwnerA);

        Assert.Multiple(() =>
                {
                    Assert.That(result.Id, Is.EqualTo(100));
                    Assert.That(result.Title, Is.EqualTo("Tarea SImple 1"));
                    Assert.That(result.UserId, Is.EqualTo(_taskOwnerA.CurrentUserId));
                    Assert.That(result.TaskType, Is.EqualTo(TaskType.SimpleTask));
                });

        _mockRepository.Verify(i => i.CreateTask(It.Is<SimpleTask>(t =>
            t.Title == "Tarea SImple 1" &&
            t.UserId == _taskOwnerA.CurrentUserId &&
            t.TaskPriority == Priority.Low)), Times.Once);
    }

    [Test]
    public void CreateTaskWithPastDateTime_MustThrowInvalidDate()
    {
        var taskDto = new CreateSimpleTaskDto
        {
            Title = "Tarea SImple Fecha Oasada",
            TaskDescription = "Descripción pruebas tarea simple pasada",
            TaskPriority = Enums.Priority.High,
            DueTime = DateTime.UtcNow.AddDays(-15)
        };

        Assert.Throws<ArgumentException>(() => _taskService.CreateTask(taskDto, _taskOwnerA));

        _mockRepository.Verify(i => i.CreateTask(It.IsAny<SimpleTask>()), Times.Never);
    }
    [Test]
    public void CreateTaskWithNotValidDueDateTime_MoreThanThreeYears_MustThrowInvalidDate()
    {
        var taskDto = new CreateSimpleTaskDto
        {
            Title = "Tarea SImple Fecha Muy Muy Lejana",
            TaskDescription = "Descripción pruebas tarea simple muy  muy lejana",
            TaskPriority = Enums.Priority.High,
            DueTime = DateTime.UtcNow.AddYears(3).AddDays(30)
        };

        _mockRepository
    .Setup(r => r.CreateTask(It.IsAny<Task>()))
    .Returns((Task task) =>
    {
        task.Id = 100;
        return task;
    });

        Assert.Throws<ArgumentException>(() => _taskService.CreateTask(taskDto, _taskOwnerA));

        _mockRepository.Verify(i => i.CreateTask(It.IsAny<SimpleTask>()), Times.Never);
    }
    [Test]
    public void CreateSimpleTask_WithoutValidUser_ThrowsUnauthorizedException()
    {
        var taskDto = new CreateSimpleTaskDto
        {
            Title = "Tarea SImple Sin User Válido",
            TaskDescription = "Descripción pruebas tarea simple usuario no válido",
            TaskPriority = Enums.Priority.High,
            DueTime = DateTime.UtcNow.AddDays(30)
        };

        Assert.Throws<UnauthorizedAccessException>(() => _taskService.CreateTask(taskDto, null!));
    }

    [Test]
    public void UpdateSimpleTask_OkOwner()
    {
        var newTask = Simple(id: 1, ownerId: _taskOwnerA.CurrentUserId);
        var updateDto = UpdateDto(title: "Actualizada propio dueño");

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);



        _taskService.UpdateTask(newTask.Id, updateDto, _taskOwnerA);

        Assert.That(newTask.Title, Is.EqualTo("Actualizada propio dueño"));
        _mockRepository.Verify(r => r.UpdateTask(newTask), Times.Once);
    }

    [Test]
    public void UpdateSimpleTask_WhitNotOwner_ThrowsUnauthorizedAccessException()
    {
        var newTask = Simple(id: 1, ownerId: _taskOwnerA.CurrentUserId);
        var updateDto = UpdateDto(title: "Intento No válido de actualizar.");

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);

        Assert.Throws<UnauthorizedAccessException>(() => _taskService.UpdateTask(newTask.Id, updateDto, _taskOwnerB));

        _mockRepository.Verify(r => r.UpdateTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void UpdateSimpleTask_WhitSystemAdmin()
    {
        var newTask = Simple(id: 1, ownerId: _taskOwnerA.CurrentUserId);
        var updateDto = UpdateDto(title: "Actualizada por ADMIN");

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);

        _taskService.UpdateTask(newTask.Id, updateDto, _adminUser);

        Assert.That(newTask.Title, Is.EqualTo("Actualizada por ADMIN"));
        _mockRepository.Verify(r => r.UpdateTask(newTask), Times.Once);
    }

    [Test]
    public void DeleteTask_WithDependencies_ThrowsInvalidOperationException()
    {
        var newTask = Simple(id: 1, ownerId: _taskOwnerB.CurrentUserId);
        newTask.Dependencies.Add(new LinkedTask
        {
            Id = 10,
            TaskId = newTask.Id,
            DependsOnTaskId = 2,
            DependsOnTask = Simple(id: 2, ownerId: _taskOwnerB.CurrentUserId)
        });

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);

        Assert.Throws<InvalidOperationException>(() => _taskService.DeleteTask(newTask.Id, _taskOwnerB));

        _mockRepository.Verify(r => r.DeleteTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void UpdateCollaborativeTask_WhithRoleCollaborator_EditOk()
    {
        var newTask = Collaborative(id: 2, ownerId: _taskOwnerA.CurrentUserId,
            Collaborator(_taskOwnerA.CurrentUserId, CollaboratorRole.TaskAdministrator),
            Collaborator(_taskOwnerB.CurrentUserId, CollaboratorRole.Collaborator));

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);

        _taskService.UpdateTask(newTask.Id, UpdateDto(title: "Escribe el colaborador"), _taskOwnerB);

        Assert.That(newTask.Title, Is.EqualTo("Escribe el colaborador"));
        _mockRepository.Verify(r => r.UpdateTask(newTask), Times.Once);
    }

    [Test]
    public void AddTaskCollaborator_WhenItsOwner_AddsCollaboratorOk()
    {
        var newTask = Collaborative(id: 2, ownerId: _taskOwnerA.CurrentUserId,
            Collaborator(_taskOwnerA.CurrentUserId, CollaboratorRole.TaskAdministrator));

        var taskCollaboratorDto = new CreateTaskCollaboratorDto
        {
            UserId = _taskOwnerB.CurrentUserId,
            CollaboratorRole = CollaboratorRole.Viewer
        };

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);
        _mockRepository.Setup(r => r.AlreadyExistsCollaborator(newTask.Id, _taskOwnerB.CurrentUserId)).Returns(false);
        _mockUserRepository.Setup(r => r.GetUserById(_taskOwnerB.CurrentUserId)).Returns(ActiveUser(_taskOwnerB.CurrentUserId));

        _taskService.AddTaskCollaborator(newTask.Id, taskCollaboratorDto, _taskOwnerA);

        _mockRepository.Verify(r => r.AddTaskCollaborator(newTask, It.Is<TaskCollaborator>(tc =>
            tc.TaskId == newTask.Id &&
            tc.UserId == _taskOwnerB.CurrentUserId &&
            tc.CollaboratorRole == CollaboratorRole.Viewer)), Times.Once);
    }

    [Test]
    public void AddTaskCollaborator_WhenNotRoleAuthWantToAddAnotherUser_ThrowsUnauthorizedAccessException()
    {
        var newTask = Collaborative(id: 2, ownerId: _taskOwnerA.CurrentUserId,
            Collaborator(_taskOwnerA.CurrentUserId, CollaboratorRole.TaskAdministrator),
            Collaborator(_taskOwnerB.CurrentUserId, CollaboratorRole.Viewer));

        var taskCollaboratorDto = new CreateTaskCollaboratorDto
        {
            UserId = _taskOwnerC.CurrentUserId,
            CollaboratorRole = CollaboratorRole.Viewer
        };

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);

        Assert.Throws<UnauthorizedAccessException>(() => _taskService.AddTaskCollaborator(newTask.Id, taskCollaboratorDto, _taskOwnerB));

        _mockRepository.Verify(r => r.AddTaskCollaborator(It.IsAny<CollaborativeTask>(), It.IsAny<TaskCollaborator>()), Times.Never);
    }

    [Test]
    public void AddTaskCollaborator_WhenRoleISAdministratorAddsCollaborator_AddsCollaborator()
    {
        var newTask = Collaborative(id: 2, ownerId: _taskOwnerA.CurrentUserId,
            Collaborator(_taskOwnerA.CurrentUserId, CollaboratorRole.TaskAdministrator),
            Collaborator(_taskOwnerB.CurrentUserId, CollaboratorRole.TaskAdministrator));

        var taskCollaboratorDto = new CreateTaskCollaboratorDto
        {
            UserId = _taskOwnerC.CurrentUserId,
            CollaboratorRole = CollaboratorRole.Viewer
        };

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);
        _mockRepository.Setup(r => r.AlreadyExistsCollaborator(newTask.Id, _taskOwnerC.CurrentUserId)).Returns(false);
        _mockUserRepository.Setup(r => r.GetUserById(_taskOwnerC.CurrentUserId)).Returns(ActiveUser(_taskOwnerC.CurrentUserId));

        _taskService.AddTaskCollaborator(newTask.Id, taskCollaboratorDto, _taskOwnerB);

        _mockRepository.Verify(r => r.AddTaskCollaborator(newTask, It.Is<TaskCollaborator>(tc =>
            tc.UserId == _taskOwnerC.CurrentUserId &&
            tc.CollaboratorRole == CollaboratorRole.Viewer)), Times.Once);
    }

    [Test]
    public void AddTaskCollaborator_WhenCollaboratorAlreadyExists_ThrowsInvalidOperationException()
    {
        var newTask = Collaborative(id: 2, ownerId: _taskOwnerA.CurrentUserId,
            Collaborator(_taskOwnerA.CurrentUserId, CollaboratorRole.TaskAdministrator),
            Collaborator(_taskOwnerB.CurrentUserId, CollaboratorRole.Viewer));

        var taskCollaboratorDto = new CreateTaskCollaboratorDto
        {
            UserId = _taskOwnerB.CurrentUserId,
            CollaboratorRole = CollaboratorRole.Viewer
        };

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);
        _mockRepository.Setup(r => r.AlreadyExistsCollaborator(newTask.Id, _taskOwnerB.CurrentUserId)).Returns(true);

        Assert.Throws<InvalidOperationException>(() => _taskService.AddTaskCollaborator(newTask.Id, taskCollaboratorDto, _taskOwnerA));

        _mockRepository.Verify(r => r.AddTaskCollaborator(It.IsAny<CollaborativeTask>(), It.IsAny<TaskCollaborator>()), Times.Never);
    }

    [Test]
    public void RemoveTaskCollaborator_TryingRemoveOwner_ShouldThrowInvalidOperationException()
    {
        var newTask = Collaborative(id: 2, ownerId: _taskOwnerA.CurrentUserId,[
            Collaborator(_taskOwnerA.CurrentUserId, CollaboratorRole.TaskAdministrator),
            Collaborator(_taskOwnerB.CurrentUserId,CollaboratorRole.Collaborator)]);
            

        _mockRepository.Setup(r => r.GetTaskById(newTask.Id)).Returns(newTask);
        _mockUserRepository.Setup(r => r.GetUserById(_taskOwnerB.CurrentUserId)).Returns(ActiveUser(_taskOwnerB.CurrentUserId));

        Assert.Throws<UnauthorizedAccessException>(() =>
            _taskService.RemoveTaskCollaborator(newTask.Id, _taskOwnerA.CurrentUserId, _taskOwnerB));

        _mockRepository.Verify(r => r.RemoveTaskCollaborator(It.IsAny<CollaborativeTask>(), It.IsAny<TaskCollaborator>()), Times.Never);
    }

    //COMPOSITETASKS

    [Test]
    public void CreateSubTask_WhenParentIsNotComposite_ThrowsInvalidOperationException()
    {
        var parentTask = Simple(id: 1, ownerId: _taskOwnerA.CurrentUserId);
        _mockRepository.Setup(r => r.GetTaskById(parentTask.Id)).Returns(parentTask);

        Assert.Throws<InvalidOperationException>(() =>
            _taskService.CreateSubTask(parentTask.Id, CreateSubTaskDto(), _taskOwnerA));

        _mockRepository.Verify(r => r.CreateTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void CreateSubTask_WhenUserIsNotTheOwner_ThrowsInvalidOperationException()
    {
        var parentTask = Composite(id: 3, ownerId: _taskOwnerA.CurrentUserId);
        _mockRepository.Setup(r => r.GetTaskById(parentTask.Id)).Returns(parentTask);

        Assert.Throws<UnauthorizedAccessException>(() =>
            _taskService.CreateSubTask(parentTask.Id, CreateSubTaskDto(), _taskOwnerB));

        _mockRepository.Verify(r => r.CreateTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void CreateSubTask_WhitUserOwnerAndParentIsComposite_CreatesSubTask()
    {
        var parentTask = Composite(id: 3, ownerId: _taskOwnerA.CurrentUserId);
        var subTaskDto = CreateSubTaskDto("Subtarea Pruebas Ok 1");

        _mockRepository.Setup(r => r.GetTaskById(parentTask.Id)).Returns(parentTask);
         _mockRepository
        .Setup(r => r.CreateTask(It.IsAny<Task>()))
        .Returns((Task task) =>
        {
            task.Id = 111;
            return task;
        });

        var result = _taskService.CreateSubTask(parentTask.Id, subTaskDto, _taskOwnerA);

        Assert.Multiple(() =>
        {
            Assert.That(result.Title, Is.EqualTo("Subtarea Pruebas Ok 1"));
            Assert.That(result.UserId, Is.EqualTo(_taskOwnerA.CurrentUserId));
            Assert.That(result.TaskType, Is.EqualTo(TaskType.SubTask));
        });

        _mockRepository.Verify(r => r.CreateTask(It.Is<SubTask>(st =>
            st.ParentCompositeTaskId == parentTask.Id &&
            st.UserId == _taskOwnerA.CurrentUserId)), Times.Once);
    }

    [Test]
    public void CompleteCompositeTask_WithSubTasksNotCompleted_ThrowsInvalidOperationException()
    {
        var parentTask = Composite(id: 3, ownerId: _taskOwnerA.CurrentUserId,
            SubTask(id: 4, ownerId: _taskOwnerA.CurrentUserId, parentId: 3, taskStatus: TaskStatus.Pending),
            SubTask(id: 5, ownerId: _taskOwnerA.CurrentUserId, parentId: 3, taskStatus: TaskStatus.Completed));

        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(parentTask.Id)).Returns(parentTask);

        Assert.Throws<InvalidOperationException>(() => _taskService.CompleteTask(parentTask.Id, _taskOwnerA));

        _mockRepository.Verify(r => r.UpdateTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void CompleteCompositeTask_WhenAllSubTasksCompletedOk_CompleteOk()
    {
        var parentTask = Composite(id: 3, ownerId: _taskOwnerA.CurrentUserId,
            SubTask(id: 4, ownerId: _taskOwnerA.CurrentUserId, parentId: 3, taskStatus: TaskStatus.Completed),
            SubTask(id: 5, ownerId: _taskOwnerA.CurrentUserId, parentId: 3, taskStatus: TaskStatus.Completed));

        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(parentTask.Id)).Returns(parentTask);

        Assert.DoesNotThrow(() => _taskService.CompleteTask(parentTask.Id, _taskOwnerA));

        Assert.That(parentTask.TaskStatus, Is.EqualTo(TaskStatus.Completed));
        _mockRepository.Verify(r => r.UpdateTask(parentTask), Times.Once);
    }

    //LINKEDTASK

    [Test]
    public void AddLinkedTask_WhenTaskIsTheSame_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _taskService.AddLinkedTask(
            taskId: 10,
            dependsOnTaskId: 10,
            linkedTaskOrder: 1,
            currentUserDto: _taskOwnerA));

        _mockRepository.Verify(r => r.AddLinkedRelation(It.IsAny<LinkedTask>()), Times.Never);
    }

    [Test]
    public void AddLinkedTask_WhenCircularRelationExists_ThrowsInvalidOperationException()
    {
        var taskA = Simple(id: 10, ownerId: _taskOwnerA.CurrentUserId);
        var taskB = Simple(id: 11, ownerId: _taskOwnerA.CurrentUserId);

        _mockRepository.Setup(r => r.GetTaskById(taskA.Id)).Returns(taskA);
        _mockRepository.Setup(r => r.GetTaskById(taskB.Id)).Returns(taskB);
        _mockRepository.Setup(r => r.ExistsCircularRelation(taskA.Id, taskB.Id)).Returns(true);

        Assert.Throws<InvalidOperationException>(() => _taskService.AddLinkedTask(taskA.Id, taskB.Id, 1, _taskOwnerA));

        _mockRepository.Verify(r => r.AddLinkedRelation(It.IsAny<LinkedTask>()), Times.Never);
    }

    [Test]
    public void AddLinkedTask_WhenRelationAlreadyExists_ThrowsInvalidOperationException()
    {
        var taskA = Simple(id: 10, ownerId: _taskOwnerA.CurrentUserId);
        var taskB = Simple(id: 11, ownerId: _taskOwnerA.CurrentUserId);

        _mockRepository.Setup(r => r.GetTaskById(taskA.Id)).Returns(taskA);
        _mockRepository.Setup(r => r.GetTaskById(taskB.Id)).Returns(taskB);
        _mockRepository.Setup(r => r.ExistsCircularRelation(taskA.Id, taskB.Id)).Returns(false);
        _mockRepository.Setup(r => r.ExistsLinkedRelation(taskA.Id, taskB.Id)).Returns(true);

        Assert.Throws<InvalidOperationException>(() => _taskService.AddLinkedTask(taskA.Id, taskB.Id, 1, _taskOwnerA));

        _mockRepository.Verify(r => r.AddLinkedRelation(It.IsAny<LinkedTask>()), Times.Never);
    }

    [Test]
    public void AddLinkedTask_WhenValid_AddsRelationOk()
    {
        var taskA = Simple(id: 10, ownerId: _taskOwnerA.CurrentUserId);
        var taskB = Simple(id: 11, ownerId: _taskOwnerA.CurrentUserId);

        _mockRepository.Setup(r => r.GetTaskById(taskA.Id)).Returns(taskA);
        _mockRepository.Setup(r => r.GetTaskById(taskB.Id)).Returns(taskB);
        _mockRepository.Setup(r => r.ExistsCircularRelation(taskA.Id, taskB.Id)).Returns(false);
        _mockRepository.Setup(r => r.ExistsLinkedRelation(taskA.Id, taskB.Id)).Returns(false);
        _mockRepository.Setup(r => r.AddLinkedRelation(It.IsAny<LinkedTask>()))
            .Returns((LinkedTask relation) =>
            {
                relation.Id = 54;
                return relation;
            });

        var result = _taskService.AddLinkedTask(taskA.Id, taskB.Id, 1, _taskOwnerA);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(54));
            Assert.That(result.TaskId, Is.EqualTo(taskA.Id));
            Assert.That(result.DependsOnTaskId, Is.EqualTo(taskB.Id));
            Assert.That(result.LinkedTaskOrder, Is.EqualTo(1));
        });

        _mockRepository.Verify(r => r.AddLinkedRelation(It.Is<LinkedTask>(lt =>
            lt.TaskId == taskA.Id &&
            lt.DependsOnTaskId == taskB.Id &&
            lt.LinkedTaskOrder == 1)), Times.Once);
    }

    [Test]
    public void AddLinkedTask_WithNotTheSameOwner_ThrowsUnauthorizedAccessException()
    {
        var taskA = Simple(id: 10, ownerId: _taskOwnerA.CurrentUserId);
        var taskB = Simple(id: 11, ownerId: _taskOwnerB.CurrentUserId);

        _mockRepository.Setup(r => r.GetTaskById(taskA.Id)).Returns(taskA);
        _mockRepository.Setup(r => r.GetTaskById(taskB.Id)).Returns(taskB);

        Assert.Throws<UnauthorizedAccessException>(() => _taskService.AddLinkedTask(taskA.Id, taskB.Id, 1, _taskOwnerA));

        _mockRepository.Verify(r => r.AddLinkedRelation(It.IsAny<LinkedTask>()), Times.Never);
    }

    [Test]
    public void CompleteLinkedTask_WhenDependencyPending_ThrowsInvalidOperationException()
    {
        var dependsTask = Simple(id: 11, ownerId: _taskOwnerA.CurrentUserId, taskStatus: TaskStatus.Pending);
        var taskA = Simple(id: 10, ownerId: _taskOwnerA.CurrentUserId);
        taskA.Dependencies.Add(new LinkedTask
        {
            Id = 36,
            TaskId = taskA.Id,
            DependsOnTaskId = dependsTask.Id,
            Task = taskA,
            DependsOnTask = dependsTask,
            LinkedTaskOrder = 1
        });

        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(taskA.Id)).Returns(taskA);

        Assert.Throws<InvalidOperationException>(() => _taskService.CompleteTask(taskA.Id, _taskOwnerA));

        _mockRepository.Verify(r => r.UpdateTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void CompleteLinkedTask_WhenDependencyCompleted_CompletesAndPersists()
    {
        var dependsTask = Simple(id: 11, ownerId: _taskOwnerA.CurrentUserId, taskStatus: TaskStatus.Completed);
        var taskA = Simple(id: 10, ownerId: _taskOwnerA.CurrentUserId);
        taskA.Dependencies.Add(new LinkedTask
        {
            Id = 98,
            TaskId = taskA.Id,
            DependsOnTaskId = dependsTask.Id,
            Task = taskA,
            DependsOnTask = dependsTask,
            LinkedTaskOrder = 1
        });

        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(taskA.Id)).Returns(taskA);

        Assert.DoesNotThrow(() => _taskService.CompleteTask(taskA.Id, _taskOwnerA));

        Assert.That(taskA.TaskStatus, Is.EqualTo(TaskStatus.Completed));
        _mockRepository.Verify(r => r.UpdateTask(taskA), Times.Once);
    }

    //RECURRINGTASK

    [Test]
    public void CreateRecurringTask_WithFutureStartDateAndLimit_GeneratesOk()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var recurringTaskDto = new CreateRecurringTaskDto
        {
            Title = "Recurrencia semanal",
            TaskDescription = "Iteracion semanal de pruebas",
            TaskPriority = Priority.Normal,
            TaskType = TaskType.RecurringTask,
            DueTime = startDate,
            RecurrenceRule = 7,
            RepeatUntilDate = startDate.AddDays(49),
            MaxOcurrences = 10
        };

        var itemsId = 200;
        _mockRepository.Setup(r => r.CreateTask(It.IsAny<Task>()))
            .Returns((Task newTask) =>
            {
                newTask.Id = itemsId++;
                return newTask;
            });

        var result = _taskService.CreateRecurringTask(recurringTaskDto, _taskOwnerA);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(8));
            Assert.That(result.Select(r => r.DueTime), Is.EqualTo(new DateTime?[]
            {
                startDate,
                startDate.AddDays(7),
                startDate.AddDays(14),
                startDate.AddDays(21),
                startDate.AddDays(28),
                startDate.AddDays(35),
                startDate.AddDays(42),
                startDate.AddDays(49),
            }));
            Assert.That(result.Select(r => r.RecurringTasksCount), Is.EqualTo(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
        });

        _mockRepository.Verify(r => r.CreateTask(It.IsAny<RecurringTask>()), Times.Exactly(8));
    }

    [Test]
    public void CreateRecurringTask_WhitNotValidStart_ThrowsArgumentException()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var recurringTaskDto = new CreateRecurringTaskDto
        {
            Title = "Tarea recurrente daatos no válidos.",
            DueTime = startDate,
            RecurrenceRule = 7,
            RepeatUntilDate = startDate.AddDays(-1),
            MaxOcurrences = 5
        };

        Assert.Throws<ArgumentException>(() => _taskService.CreateRecurringTask(recurringTaskDto, _taskOwnerA));

        _mockRepository.Verify(r => r.CreateTask(It.IsAny<Task>()), Times.Never);
    }

    [TestCase(0)]
    [TestCase(101)]
    public void CreateRecurringTask_WhenMaxOccurrencesIsOutOfRange_ThrowsArgumentException(int maxOccurrences)
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var recurringTaskDto = new CreateRecurringTaskDto
        {
            Title = "Otra tarea recurrente con datos incorrectos.",
            DueTime = startDate,
            RecurrenceRule = 7,
            RepeatUntilDate = startDate.AddDays(14),
            MaxOcurrences = maxOccurrences
        };

        Assert.Throws<ArgumentException>(() => _taskService.CreateRecurringTask(recurringTaskDto, _taskOwnerA));

        _mockRepository.Verify(r => r.CreateTask(It.IsAny<Task>()), Times.Never);
    }

    [Test]
    public void CreateRecurringTask_WithPastStartDate_ShouldThrowArgumentException()
    {
        var startDate = DateTime.UtcNow.AddDays(-15);
        var recurringTaskDto = new CreateRecurringTaskDto
        {
            Title = "Recurrente con la fecha de inicio pasada unos días.",
            DueTime = startDate,
            RecurrenceRule = 7,
            RepeatUntilDate = DateTime.UtcNow.AddDays(28),
            MaxOcurrences = 4
        };

        Assert.Throws<ArgumentException>(() => _taskService.CreateRecurringTask(recurringTaskDto, _taskOwnerA));

        _mockRepository.Verify(r => r.CreateTask(It.IsAny<Task>()), Times.Never);
    }

    //CONSULTAS
    [Test]
    public void GetTaskById_WhenUserIsOwner_ShouldReturnTask()
    {
        var newTask = Simple(id: 1, ownerId: _taskOwnerA.CurrentUserId);
        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(newTask.Id)).Returns(newTask);

        var result = _taskService.GetTaskById(newTask.Id, _taskOwnerA);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(newTask.Id));
        Assert.That(result.Title, Is.EqualTo(newTask.Title));
    }

    [Test]
    public void GetTaskById_WhenTaskDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(999)).Returns((Task?)null);

        Assert.Throws<KeyNotFoundException>(() => _taskService.GetTaskById(999, _taskOwnerC));
    }

    [Test]
    public void GetTaskById_WhenDifferentUserAndNotCollaborator_ShouldThrowUnauthorizedAccessException()
    {
        var newTask = Simple(id: 1, ownerId: _taskOwnerC.CurrentUserId);
        _mockRepository.Setup(r => r.GetTaskByIdWithRelations(newTask.Id)).Returns(newTask);

        Assert.Throws<UnauthorizedAccessException>(() => _taskService.GetTaskById(newTask.Id, _taskOwnerB));
    }

}
